using DoAn_DAPM.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace DoAn_DAPM.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        private QlyGiayEntities db = new QlyGiayEntities();
        public ActionResult Index()
        {
            return View(db.NguoiDungs.ToList());
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "MaNguoiDung,TenNguoiDung,TenDangNhap,MatKhau,Email,SDT,DiaChi")] NguoiDung nguoiDung)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(nguoiDung.TenNguoiDung) ||
                    string.IsNullOrWhiteSpace(nguoiDung.TenDangNhap) ||
                    string.IsNullOrWhiteSpace(nguoiDung.MatKhau) ||
                    string.IsNullOrWhiteSpace(nguoiDung.Email) ||
                    string.IsNullOrWhiteSpace(nguoiDung.SDT) ||
                    string.IsNullOrWhiteSpace(nguoiDung.DiaChi))
                {
                    ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin.");
                    return View(nguoiDung);
                }
                if (nguoiDung.MatKhau.Length < 6)
                {
                    ModelState.AddModelError("MatKhau", "Mật khẩu phải chứa ít nhất 6 kí tự.");
                    return View(nguoiDung);
                }

  
                if (!nguoiDung.Email.EndsWith("@gmail.com"))
                {
                    ModelState.AddModelError("Email", "Email phải kết thúc bằng '@gmail.com'.");
                    return View(nguoiDung);
                }

                if (!Regex.IsMatch(nguoiDung.SDT, "^0\\d{9}$"))
                {
                    ModelState.AddModelError("SDT", "Số điện thoại phải bắt đầu bằng số 0 và có 10 chữ số.");
                    return View(nguoiDung);
                }
                db.NguoiDungs.Add(nguoiDung);
                db.SaveChanges();
                Session["KH"] = nguoiDung;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Tên tài khoản đã được sử dụng!");
            }
            return View(nguoiDung);
        }

        public ActionResult CaNhan()
        {
            NguoiDung nd = new NguoiDung();
            if (Session["KH"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                nd = (NguoiDung)Session["KH"];
            }
            return View(nd);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CaNhan([Bind(Include = "MaNguoiDung,TenNguoiDung,TenDangNhap,MatKhau,Email,SDT,DiaChi")] NguoiDung nguoiDung)
        {
            if (ModelState.IsValid)
            {
                if (nguoiDung.MatKhau.Length < 6)
                {
                    ModelState.AddModelError("MatKhau", "Mật khẩu phải chứa ít nhất 6 kí tự.");
                    return View(nguoiDung);
                }

                if (!nguoiDung.Email.EndsWith("@gmail.com"))
                {
                    ModelState.AddModelError("Email", "Email phải kết thúc bằng '@gmail.com'.");
                    return View(nguoiDung);
                }

                if (!Regex.IsMatch(nguoiDung.SDT, "^0\\d{9}$"))
                {
                    ModelState.AddModelError("SDT", "Số điện thoại phải bắt đầu bằng số 0 và có 10 chữ số.");
                    return View(nguoiDung);
                }
                var existingUser = db.NguoiDungs.Find(nguoiDung.MaNguoiDung);
                if (existingUser != null)
                {
                    db.Entry(existingUser).CurrentValues.SetValues(nguoiDung);
                    db.SaveChanges();
                    Session["KH"] = existingUser;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return HttpNotFound();
                }
            }
            return View(nguoiDung);
        }


      

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(NguoiDung nguoiDung)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(nguoiDung.TenDangNhap))
                {
                    ModelState.AddModelError("", "Vui lòng nhập tên đăng nhập");
                }
                else if (string.IsNullOrEmpty(nguoiDung.MatKhau))
                {
                    ModelState.AddModelError("", "Vui lòng nhập mật khẩu.");
                }
                if (ModelState.IsValid)
                {
                    var tk = db.NguoiDungs.FirstOrDefault(k => k.TenDangNhap == nguoiDung.TenDangNhap && k.MatKhau == nguoiDung.MatKhau);
                    if (tk != null)
                    {
                        Session["KH"] = tk;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Đăng nhập không chính xác!");
                    }
                }
            }
            return View(nguoiDung);
        }
        [HttpGet]
        public ActionResult Login()
        {
            Session["KH"] = null;
            NguoiDung kh = (NguoiDung)Session["KH"];
            if (kh != null)
                return RedirectToAction("Index", "Home");
            return View();
        }

        public ActionResult Logout()
        {
            Session["KH"] = null;
            return RedirectToAction("Login", "Account");
        }

        public ActionResult DanhSachDonHang()
        {
            NguoiDung nd = (NguoiDung)Session["KH"];
            int userId = nd.MaNguoiDung;
            var danhSachDonHang = db.DonHangs.Where(d => d.MaNguoiDung == userId && !d.DaNhanHang).ToList();
            return View(danhSachDonHang);
        }


        public ActionResult ChiTietDonHang(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = db.DonHangs.Include(d => d.NguoiDung).FirstOrDefault(d => d.MaDonHang == id); 
            if (donHang == null)
            {
                return HttpNotFound();
            }
            var chiTietDonHang = db.ChiTietDonHangs.Where(c => c.MaDonHang == id).ToList();
            donHang.ChiTietDonHangs = chiTietDonHang;
            decimal gia = donHang.ChiTietDonHangs.Select(ctdh => ctdh.Gia).FirstOrDefault();
            ViewBag.Gia = gia;

            return View(donHang);
        }
        public ActionResult NhanHang(int id)
        {
            DonHang donHang = db.DonHangs.Find(id);
            if (donHang != null && donHang.TinhTrang == "Đang giao")
            {
                donHang.DaNhanHang = true;
                donHang.TinhTrang = "Giao hàng thành công";
                db.Entry(donHang).State = EntityState.Modified;
                db.SaveChanges();
            }      
            return RedirectToAction("DanhSachDonHang");
        }

    }
}