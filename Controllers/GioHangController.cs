using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TranVanCuong.SachOnline.Models;

namespace TranVanCuong.SachOnline.Controllers
{
    public class GioHangController : Controller
    {
        SachOnlineEntities data = new SachOnlineEntities();

        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;

            if (lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }

            return lstGioHang;
        }

        public ActionResult ThemGioHang(int ms, string url)
        {
            List<GioHang> lstGioHang = LayGioHang();

            GioHang sp = lstGioHang.SingleOrDefault(n => n.iMaSach == ms);

            if (sp == null)
            {
                sp = new GioHang(ms);
                lstGioHang.Add(sp);
            }
            else
            {
                sp.iSoLuong++;
            }

            return Redirect(url);
        }

        private int TongSoLuong()
        {
            int tong = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;

            if (lstGioHang != null)
            {
                tong = lstGioHang.Sum(n => n.iSoLuong);
            }

            return tong;
        }

        private double TongTien()
        {
            double tong = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;

            if (lstGioHang != null)
            {
                tong = lstGioHang.Sum(n => n.dThanhTien);
            }

            return tong;
        }

        public ActionResult GioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();

            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "SachOnline");
            }

            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();

            return View(lstGioHang);
        }

        public ActionResult GioHangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();

            return PartialView();
        }

        public ActionResult XoaGioHang(int iMaSach)
        {
            List<GioHang> lstGioHang = LayGioHang();

            GioHang sp = lstGioHang.SingleOrDefault(n => n.iMaSach == iMaSach);

            if (sp != null)
            {
                lstGioHang.RemoveAll(n => n.iMaSach == iMaSach);

                if (lstGioHang.Count == 0)
                {
                    return RedirectToAction("Index", "SachOnline");
                }
            }

            return RedirectToAction("GioHang");
        }

        public ActionResult CapNhatGioHang(int iMaSach, FormCollection f)
        {
            List<GioHang> lstGioHang = LayGioHang();

            GioHang sp = lstGioHang.SingleOrDefault(n => n.iMaSach == iMaSach);

            if (sp != null)
            {
                sp.iSoLuong = int.Parse(f["txtSoLuong"].ToString());
            }

            return RedirectToAction("GioHang");
        }

        public ActionResult XoaTatCaGioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            lstGioHang.Clear();

            return RedirectToAction("Index", "SachOnline");
        }

        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null)
            {
                return RedirectToAction("DangNhap", "User");
            }

            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "SachOnline");
            }

            List<GioHang> lstGioHang = LayGioHang();

            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();

            return View(lstGioHang);
        }

        [HttpPost]
        public ActionResult DatHang(FormCollection f)
        {
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["TaiKhoan"];
            List<GioHang> gh = LayGioHang();

            ddh.MaKH = kh.MaKH;
            ddh.NgayDat = DateTime.Now;

            string ngayGiao = f["NgayGiao"];
            if (!string.IsNullOrEmpty(ngayGiao))
            {
                ddh.NgayGiao = DateTime.Parse(ngayGiao);
            }

            ddh.DaThanhToan = false;
            ddh.TinhTrangGiaoHang = 0;

            data.DONDATHANG.Add(ddh);
            data.SaveChanges();

            foreach (var item in gh)
            {
                CHITIETDATHANG ctdh = new CHITIETDATHANG();

                ctdh.MaDonHang = ddh.MaDonHang;
                ctdh.MaSach = item.iMaSach;
                ctdh.SoLuong = item.iSoLuong;
                ctdh.DonGia = (decimal)item.dDonGia;

                data.CHITIETDATHANG.Add(ctdh);
            }

            data.SaveChanges();

            Session["GioHang"] = null;

            return RedirectToAction("XacNhanDonHang", "GioHang");
        }

        public ActionResult XacNhanDonHang()
        {
            return View();
        }
    }
}