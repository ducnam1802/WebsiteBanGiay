using DoAn_DAPM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn_DAPM.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        QlyGiayEntities db = new QlyGiayEntities();
        // GET: Admin/Login
        public ActionResult Index()
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login");
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(NhanVien admin)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(admin.TaiKhoan))
                    ModelState.AddModelError(string.Empty, "User name không được để trống");
                if (string.IsNullOrEmpty(admin.MatKhau))
                    ModelState.AddModelError(string.Empty, "Password không được để trống");
                //Kiểm tra có admin này hay chưa
                var adminDB = db.NhanViens.FirstOrDefault(ad => ad.TaiKhoan == admin.TaiKhoan && ad.MatKhau == admin.MatKhau);
                if (adminDB == null)
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không đúng");
                else
                {
                    Session["Admin"] = adminDB;
                    ViewBag.ThongBao = "Đăng nhập admin thành công";
                    return RedirectToAction("Index", "Admin");
                }
            }

            return View();
        }
        public ActionResult Logout()
        {
            Session["Admin"] = "";
            return Redirect("~/Admin/Login");
        }
    }
}