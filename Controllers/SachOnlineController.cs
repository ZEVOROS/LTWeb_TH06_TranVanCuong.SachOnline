using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TranVanCuong.SachOnline.Models;

namespace TranVanCuong.SachOnline.Controllers
{
    public class SachOnlineController : Controller
    {
        SachOnlineEntities data = new SachOnlineEntities();

        private List<SACH> LaySachMoi(int count)
        {
            return data.SACH
                       .OrderByDescending(s => s.NgayCapNhat)
                       .Take(count)
                       .ToList();
        }

        public ActionResult Index()
        {
            var listSachMoi = LaySachMoi(6);
            return View(listSachMoi);
        }

        public ActionResult ChuDePartial()
        {
            var listChuDe = data.CHUDE.ToList();
            return PartialView(listChuDe);
        }

        public ActionResult NhaXuatBanPartial()
        {
            var listNXB = data.NHAXUATBAN.ToList();
            return PartialView(listNXB);
        }

        public ActionResult SachBanNhieuPartial()
        {
            var listSach = data.SACH
                               .OrderByDescending(s => s.SoLuongBan)
                               .Take(6)
                               .ToList();

            return PartialView(listSach);
        }

        public ActionResult ChiTietSach(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            var sach = data.SACH.SingleOrDefault(s => s.MaSach == id);

            if (sach == null)
                return HttpNotFound();

            return View(sach);
        }

        public ActionResult SachTheoChuDe(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            CHUDE cd = data.CHUDE
                           .SingleOrDefault(n => n.MaCD == id);

            ViewBag.TenChuDe = cd.TenChuDe;

            var sach = data.SACH
                           .Where(n => n.MaCD == id)
                           .ToList();

            return View(sach);
        }

        public ActionResult SachTheoNhaXuatBan(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            NHAXUATBAN nxb = data.NHAXUATBAN
                                 .SingleOrDefault(n => n.MaNXB == id);

            ViewBag.TenNXB = nxb.TenNXB;

            var sach = data.SACH
                           .Where(n => n.MaNXB == id)
                           .ToList();

            return View(sach);
        }

        public ActionResult NavPartial()
        {
            return PartialView();
        }

        public ActionResult SliderPartial()
        {
            return PartialView();
        }

        public ActionResult FooterPartial()
        {
            return PartialView();
        }

    }
}