using System;
using System.Linq;
using System.Web.Mvc;
using TranVanCuong.SachOnline.Models;
using System.Security.Cryptography;
using System.Text;

namespace TranVanCuong.SachOnline.Controllers
{
    public class UserController : Controller
    {
        SachOnlineEntities data = new SachOnlineEntities();

        // Hàm mã hóa MD5
        public static string GetMD5(string str)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(str);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();

            foreach (byte b in hashBytes)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }

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

                // Mã hóa mật khẩu trước khi lưu
                kh.MatKhau = GetMD5(matKhau);

                kh.Email = email;
                kh.DienThoai = dienThoai;
                kh.DiaChi = diaChi;

                if (!string.IsNullOrEmpty(ngaySinh))
                    kh.NgaySinh = DateTime.Parse(ngaySinh);

                data.KHACHHANG.Add(kh);

                try
                {
                    data.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    var errors = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => $"Thuộc tính: {x.PropertyName} - Lỗi: {x.ErrorMessage}");

                    throw new Exception(string.Join("\n", errors));
                }

                ViewBag.ThongBao = "Đăng ký thành công. Dữ liệu đã lưu vào database.";

                return RedirectToAction("DangNhap", new
                {
                    returnUrl = returnUrl
                });
            }

            return View();
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
                // Mã hóa mật khẩu trước khi kiểm tra
                string matKhauMD5 = GetMD5(matKhau);

                var kh = data.KHACHHANG.SingleOrDefault(k =>
                    k.TaiKhoan == tenDN &&
                    k.MatKhau == matKhauMD5
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
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng.";
                }
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
    }
}