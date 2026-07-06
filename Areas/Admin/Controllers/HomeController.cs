using System.Linq;
using System.Web.Mvc;
using TranVanCuong.SachOnline.Models;

namespace TranVanCuong.SachOnline.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private SachOnlineEntities data = new SachOnlineEntities();

        // Trang chủ Admin
        public ActionResult Index()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login");
            }

            ViewBag.SoSach = data.SACH.Count();
            ViewBag.SoChuDe = data.CHUDE.Count();
            ViewBag.SoNXB = data.NHAXUATBAN.Count();
            ViewBag.SoKhachHang = data.KHACHHANG.Count();
            ViewBag.SoDonHang = data.DONDATHANG.Count();

            return View();
        }

        // GET: Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            string tenDangNhap = form["UserName"];
            string matKhau = form["Password"];

            ADMIN admin = data.ADMIN.SingleOrDefault(a =>
                a.TenDN == tenDangNhap &&
                a.MatKhau == matKhau);

            if (admin != null)
            {
                Session["Admin"] = admin;
                return RedirectToAction("Index");
            }

            ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng.";
            return View();
        }

        // Logout
        public ActionResult Logout()
        {
            Session["Admin"] = null;
            return RedirectToAction("Login");
        }
    }
}