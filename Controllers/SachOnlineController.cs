using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TranVanCuong.SachOnline.Models;
using PagedList;
using PagedList.Mvc;

namespace TranVanCuong.SachOnline.Controllers
{
    public class SachOnlineController : Controller
    {
        SachOnlineEntities data = new SachOnlineEntities();


        public ActionResult Index(int? page)
        {
            int pageSize = 6;
            int pageNum = page ?? 1;

            var sachMoi = data.SACH
                             .OrderByDescending(s => s.NgayCapNhat)
                             .ToPagedList(pageNum, pageSize);

            return View(sachMoi);
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

        public ActionResult SachTheoChuDe(int id, int? page)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "SachOnline");
            }
            int pageSize = 6;
            int pageNum = page ?? 1;

            ViewBag.MaCD = id;
            ViewBag.TenCD = data.CHUDE.SingleOrDefault(cd => cd.MaCD == id).TenChuDe;

            var sach = data.SACH
                           .Where(s => s.MaCD == id)
                           .OrderBy(s => s.MaSach);

            return View(sach.ToPagedList(pageNum, pageSize));
        }

        public ActionResult SachTheoNhaXuatBan(int id, int? page)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "SachOnline");
            }
            int pageSize = 6;
            int pageNum = page ?? 1;

            ViewBag.MaNXB = id;
            ViewBag.TenNXB = data.NHAXUATBAN.SingleOrDefault(nxb => nxb.MaNXB == id).TenNXB;

            var sach = data.SACH
                           .Where(s => s.MaNXB == id)
                           .OrderBy(s => s.MaSach);

            return View(sach.ToPagedList(pageNum, pageSize));
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