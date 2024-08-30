using DoAn_DAPM.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace DoAn_DAPM.Areas.Admin.Controllers
{
    public class KhachHangController : Controller
    {
        QlyGiayEntities db = new QlyGiayEntities();
        // GET: KhachHang
        public ActionResult Index()
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Admin");
            return View(db.NguoiDungs.ToList());
        }    
        public ActionResult Add()
        {
            return View();
        }

        // POST: KhachHang/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "MaNguoiDung,TenNguoiDung,TenDangNhap,MatKhau,Email,SDT,DiaChi")] NguoiDung nguoiDung)
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
                // Kiểm tra độ dài của mật khẩu
                if (nguoiDung.MatKhau.Length < 6)
                {
                    ModelState.AddModelError("MatKhau", "Mật khẩu phải chứa ít nhất 6 kí tự.");
                    return View(nguoiDung);
                }

                // Kiểm tra định dạng email
                if (!nguoiDung.Email.EndsWith("@gmail.com"))
                {
                    ModelState.AddModelError("Email", "Email phải kết thúc bằng '@gmail.com'.");
                    return View(nguoiDung);
                }

                // Kiểm tra số điện thoại
                if (!Regex.IsMatch(nguoiDung.SDT, "^0\\d{8,}$"))
                {
                    ModelState.AddModelError("SDT", "Số điện thoại phải bắt đầu bằng số 0 và có ít nhất 9 chữ số.");
                    return View(nguoiDung);
                }
                if (db.NguoiDungs.Find(nguoiDung.MaNguoiDung) == null)
                {
                    db.NguoiDungs.Add(nguoiDung);
                    db.SaveChanges();
                    return RedirectToAction("Index", "KhachHang");
                }
                else
                {
                    ModelState.AddModelError("", "Login data is incorrect!");
                }
            }

            return View(nguoiDung);
        }

        // GET: KhachHang/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NguoiDung nguoiDung = db.NguoiDungs.Find(id);
            if (nguoiDung == null)
            {
                return HttpNotFound();
            }
            return View(nguoiDung);
        }

        // POST: KhachHang/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaNguoiDung,TenNguoiDung,TenDangNhap,MatKhau,Email,SDT,DiaChi")] NguoiDung nguoiDung)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nguoiDung).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nguoiDung);
        }
        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NguoiDung nguoiDung = db.NguoiDungs.Find(id);
            if (nguoiDung == null)
            {
                return HttpNotFound();
            }
            return View(nguoiDung);
        }

        // POST: KhachHang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                NguoiDung nguoiDung = db.NguoiDungs.Find(id);
                if (nguoiDung == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy khách hàng.";
                    return RedirectToAction("Index");
                }

                db.NguoiDungs.Remove(nguoiDung);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Khách hàng đã được xóa thành công.";
            }
            catch (Exception ex)
            {
                // Ghi log lỗi tại đây hoặc xử lý ngoại lệ
                TempData["ErrorMessage"] = "Lỗi xảy ra khi xóa khách hàng: " + ex.Message;
            }

            return RedirectToAction("Index");
        }


    }
}