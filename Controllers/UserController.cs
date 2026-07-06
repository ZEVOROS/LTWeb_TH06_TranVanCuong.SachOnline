using System;
using System.Linq;
using System.Web.Mvc;
using TranVanCuong.SachOnline.Models;

namespace TranVanCuong.SachOnline.Controllers
{
    public class UserController : Controller
    {
        SachOnlineEntities data = new SachOnlineEntities();

        [HttpGet]
        public ActionResult DangKy(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(FormCollection collection, string returnUrl)
        {
            string hoTen = collection["HoTen"];
            string tenDN = collection["TenDN"];
            string matKhau = collection["MatKhau"];
            string matKhauNL = collection["MatKhauNL"];
            string email = collection["Email"];
            string dienThoai = collection["DienThoai"];
            string ngaySinh = collection["NgaySinh"];
            string diaChi = collection["DiaChi"];

            if (string.IsNullOrEmpty(hoTen))
                ViewData["err1"] = "Họ tên không được để trống";
            else if (string.IsNullOrEmpty(tenDN))
                ViewData["err2"] = "Tên đăng nhập không được để trống";
            else if (string.IsNullOrEmpty(matKhau))
                ViewData["err3"] = "Mật khẩu không được để trống";
            else if (string.IsNullOrEmpty(matKhauNL))
                ViewData["err4"] = "Vui lòng nhập lại mật khẩu";
            else if (matKhau != matKhauNL)
                ViewData["err4"] = "Mật khẩu nhập lại không khớp";
            else if (string.IsNullOrEmpty(email))
                ViewData["err5"] = "Email không được để trống";
            else if (string.IsNullOrEmpty(dienThoai))
                ViewData["err6"] = "Điện thoại không được để trống";
            else if (data.KHACHHANG.Any(kh => kh.TaiKhoan == tenDN))
                ViewBag.ThongBao = "Tên đăng nhập đã tồn tại trong database";
            else if (data.KHACHHANG.Any(kh => kh.Email == email))
                ViewBag.ThongBao = "Email đã tồn tại trong database";
            else
            {
                KHACHHANG kh = new KHACHHANG();

                kh.HoTen = hoTen;
                kh.TaiKhoan = tenDN;
                kh.MatKhau = matKhau;
                kh.Email = email;
                kh.DienThoai = dienThoai;
                kh.DiaChi = diaChi;

                if (!string.IsNullOrEmpty(ngaySinh))
                    kh.NgaySinh = DateTime.Parse(ngaySinh);

                data.KHACHHANG.Add(kh);
                data.SaveChanges();

                ViewBag.ThongBao = "Đăng ký thành công. Dữ liệu đã lưu vào database.";
            }

            return View("DangNhap");
        }

        [HttpGet]
        public ActionResult DangNhap(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        public ActionResult DangXuat(string returnUrl)
        {
            Session["TaiKhoan"] = null;

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "SachOnline");
        }

        [HttpPost]
        public ActionResult DangNhap(FormCollection collection, string returnUrl)
        {
            string tenDN = collection["TenDN"];
            string matKhau = collection["MatKhau"];

            if (string.IsNullOrEmpty(tenDN))
            {
                ViewData["Err1"] = "Bạn chưa nhập tên đăng nhập";
            }
            else if (string.IsNullOrEmpty(matKhau))
            {
                ViewData["Err2"] = "Bạn chưa nhập mật khẩu";
            }
            else
            {
                var kh = data.KHACHHANG.SingleOrDefault(k =>
                    k.TaiKhoan == tenDN &&
                    k.MatKhau == matKhau
                );

                if (kh != null)
                {
                    Session["TaiKhoan"] = kh;

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "SachOnline");
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng trong database";
                }
            }

            return View();
        }
    }
}