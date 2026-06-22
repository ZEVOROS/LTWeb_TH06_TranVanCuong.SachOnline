using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TranVanCuong.SachOnline.Models;

namespace SachOnline.Controllers
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
            var listChuDe = from cd in data.CHUDE
                            select cd;

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

        public ActionResult BookDetail(int id)
        {
            var sach = data.SACH.SingleOrDefault(s => s.MaSach == id);
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