using DoAn_DAPM.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DoAn_DAPM.Areas.Admin.Controllers
{
    public class DonHangController : Controller
    {
        QlyGiayEntities db = new QlyGiayEntities();
        // GET: Admin/Hang
        public ActionResult Index()
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login","Admin");
            return View(db.DonHangs.ToList());
        }

        public ActionResult ChitietDH(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = db.DonHangs.FirstOrDefault(d => d.MaDonHang == id);
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
        public ActionResult XacNhanDonHang(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = db.DonHangs.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            if (donHang.TinhTrang != "Giao hàng thành công")
            {
                donHang.TinhTrang = "Đang giao";
                db.Entry(donHang).State = EntityState.Modified;
                db.SaveChanges();
                TempData["StatusMessage"] = "Đơn hàng " + donHang.MaDonHang + " đã được xác nhận và đang được giao.";
            }
            else
            {
                TempData["ErrorMessage"] = "Đơn hàng " + donHang.MaDonHang + " đã giao hàng thành công và không thể xác nhận lại.";
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult XoaDon(int id)
        {
            DonHang donHang = db.DonHangs.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            var chiTietDonHangs = db.ChiTietDonHangs.Where(ctdh => ctdh.MaDonHang == id).ToList();
            foreach (var chiTiet in chiTietDonHangs)
            {
                db.ChiTietDonHangs.Remove(chiTiet);
            }
            db.DonHangs.Remove(donHang);
            db.SaveChanges();
            TempData["SuccessMessage"] = "Đơn hàng " + id + " đã được xóa thành công.";
            return RedirectToAction("Index");
        }

    }
}