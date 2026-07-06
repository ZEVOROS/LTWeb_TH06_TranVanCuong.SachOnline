using PagedList;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TranVanCuong.SachOnline.Models;

namespace TranVanCuong.SachOnline.Areas.Admin.Controllers
{
    public class SachController : Controller
    {
        private SachOnlineEntities data = new SachOnlineEntities();

        private bool KiemTraDangNhap()
        {
            return Session["Admin"] != null;
        }

        private void TaoDropdown(int? maCD = null, int? maNXB = null)
        {
            ViewBag.MaCD = new SelectList(data.CHUDE.ToList(), "MaCD", "TenChuDe", maCD);
            ViewBag.MaNXB = new SelectList(data.NHAXUATBAN.ToList(), "MaNXB", "TenNXB", maNXB);
        }

        private string LuuAnh(HttpPostedFileBase fileUpload)
        {
            string extension = Path.GetExtension(fileUpload.FileName);

            string fileName = Guid.NewGuid().ToString("N") + extension;

            if (fileName.Length > 50)
            {
                fileName = fileName.Substring(0, 45) + extension;
            }

            string path = Path.Combine(Server.MapPath("~/Images"), fileName);

            fileUpload.SaveAs(path);

            return fileName;
        }

        public ActionResult Index(int? page)
        {
            if (!KiemTraDangNhap())
                return RedirectToAction("Login", "Home");

            int pageSize = 10;
            int pageNum = page ?? 1;

            var dsSach = data.SACH
                             .Include(s => s.CHUDE)
                             .Include(s => s.NHAXUATBAN)
                             .OrderByDescending(s => s.MaSach);

            return View(dsSach.ToPagedList(pageNum, pageSize));
        }

        public ActionResult Details(int? id)
        {
            if (!KiemTraDangNhap())
                return RedirectToAction("Login", "Home");

            if (id == null)
                return RedirectToAction("Index");

            SACH sach = data.SACH
                            .Include(s => s.CHUDE)
                            .Include(s => s.NHAXUATBAN)
                            .SingleOrDefault(s => s.MaSach == id);

            if (sach == null)
                return HttpNotFound();

            return View(sach);
        }

        public ActionResult Create()
        {
            if (!KiemTraDangNhap())
                return RedirectToAction("Login", "Home");

            TaoDropdown();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SACH sach, HttpPostedFileBase fileUpload)
        {
            if (!KiemTraDangNhap())
                return RedirectToAction("Login", "Home");

            TaoDropdown(sach.MaCD, sach.MaNXB);

            if (string.IsNullOrWhiteSpace(sach.TenSach))
            {
                ModelState.AddModelError("TenSach", "Vui lòng nhập tên sách.");
            }

            if (sach.GiaBan == null || sach.GiaBan <= 0)
            {
                ModelState.AddModelError("GiaBan", "Giá bán phải lớn hơn 0.");
            }

            if (sach.MaCD == null)
            {
                ModelState.AddModelError("MaCD", "Vui lòng chọn chủ đề.");
            }

            if (sach.MaNXB == null)
            {
                ModelState.AddModelError("MaNXB", "Vui lòng chọn nhà xuất bản.");
            }

            if (sach.NgayCapNhat == null)
            {
                sach.NgayCapNhat = DateTime.Now;
            }

            if (sach.SoLuongBan == null)
            {
                sach.SoLuongBan = 0;
            }

            if (fileUpload == null || fileUpload.ContentLength == 0)
            {
                ModelState.AddModelError("AnhBia", "Vui lòng chọn ảnh bìa.");
                ViewBag.ThongBao = "Vui lòng chọn ảnh bìa.";
                return View(sach);
            }

            try
            {
                sach.AnhBia = LuuAnh(fileUpload);

                ModelState.Remove("AnhBia");

                if (!ModelState.IsValid)
                {
                    ViewBag.ThongBao = "Dữ liệu nhập chưa hợp lệ.";
                    return View(sach);
                }

                data.SACH.Add(sach);
                data.SaveChanges();

                TempData["ThongBao"] = "Thêm sách thành công.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ThongBao = "Lỗi khi thêm sách: " + ex.Message;
                return View(sach);
            }
        }

        public ActionResult Edit(int? id)
        {
            if (!KiemTraDangNhap())
                return RedirectToAction("Login", "Home");

            if (id == null)
                return RedirectToAction("Index");

            SACH sach = data.SACH.SingleOrDefault(s => s.MaSach == id);

            if (sach == null)
                return HttpNotFound();

            TaoDropdown(sach.MaCD, sach.MaNXB);

            return View(sach);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SACH sach, HttpPostedFileBase fileUpload)
        {
            if (!KiemTraDangNhap())
                return RedirectToAction("Login", "Home");

            TaoDropdown(sach.MaCD, sach.MaNXB);

            SACH sachCu = data.SACH.SingleOrDefault(s => s.MaSach == sach.MaSach);

            if (sachCu == null)
                return HttpNotFound();

            if (string.IsNullOrWhiteSpace(sach.TenSach))
            {
                ModelState.AddModelError("TenSach", "Vui lòng nhập tên sách.");
            }

            if (sach.GiaBan == null || sach.GiaBan <= 0)
            {
                ModelState.AddModelError("GiaBan", "Giá bán phải lớn hơn 0.");
            }

            if (sach.MaCD == null)
            {
                ModelState.AddModelError("MaCD", "Vui lòng chọn chủ đề.");
            }

            if (sach.MaNXB == null)
            {
                ModelState.AddModelError("MaNXB", "Vui lòng chọn nhà xuất bản.");
            }

            if (!ModelState.IsValid)
            {
                return View(sach);
            }

            try
            {
                sachCu.TenSach = sach.TenSach;
                sachCu.MoTa = sach.MoTa;
                sachCu.GiaBan = sach.GiaBan;
                sachCu.NgayCapNhat = sach.NgayCapNhat ?? DateTime.Now;
                sachCu.SoLuongBan = sach.SoLuongBan ?? 0;
                sachCu.MaCD = sach.MaCD;
                sachCu.MaNXB = sach.MaNXB;

                if (fileUpload != null && fileUpload.ContentLength > 0)
                {
                    sachCu.AnhBia = LuuAnh(fileUpload);
                }

                data.SaveChanges();

                TempData["ThongBao"] = "Cập nhật sách thành công.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ThongBao = "Lỗi khi cập nhật sách: " + ex.Message;
                return View(sach);
            }
        }

        public ActionResult Delete(int? id)
        {
            if (!KiemTraDangNhap())
                return RedirectToAction("Login", "Home");

            if (id == null)
                return RedirectToAction("Index");

            SACH sach = data.SACH
                            .Include(s => s.CHUDE)
                            .Include(s => s.NHAXUATBAN)
                            .SingleOrDefault(s => s.MaSach == id);

            if (sach == null)
                return HttpNotFound();

            return View(sach);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Home");

            SACH sach = data.SACH.SingleOrDefault(s => s.MaSach == id);

            if (sach == null)
                return HttpNotFound();

            try
            {
                data.SACH.Remove(sach);
                data.SaveChanges();

                TempData["ThongBao"] = "Xóa sách thành công.";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ThongBao = "Không thể xóa sách.<br/>" + ex.Message;

                sach = data.SACH
                           .Include(s => s.CHUDE)
                           .Include(s => s.NHAXUATBAN)
                           .SingleOrDefault(s => s.MaSach == id);

                return View(sach);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                data.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}