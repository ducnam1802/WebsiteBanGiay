using DoAn_DAPM.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace DoAn_DAPM.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        QlyGiayEntities db = new QlyGiayEntities();
        // GET: Admin/SanPham
        public ActionResult Index(int page = 1)
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Admin");
            int pageSize = 5; 
            var sanphamQuery = db.SanPhams.Include(b => b.MauSac).Include(b => b.ThuongHieu).Include(b => b.KichCo).Include(b => b.SanPham_Anh);
            var totalCount = sanphamQuery.Count();
            var sanpham = sanphamQuery.OrderBy(sp => sp.MaSP).Skip((page - 1) * pageSize).Take(pageSize) .ToList();

            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize); 
            ViewBag.CurrentPage = page; 

            return View(sanpham); 
        }
        public ActionResult Create()
        {
            // Truy vấn danh sách màu sắc, kích cỡ và thương hiệu từ database
            var mauSacList = db.MauSacs.ToList();
            var kichCoList = db.KichCoes.ToList();
            var thuongHieuList = db.ThuongHieux.ToList();

            // Truyền danh sách vào View để hiển thị trong dropdownlist hoặc listbox
            ViewBag.MauSacList = new SelectList(mauSacList, "MaMauSac", "TenMauSac");
            ViewBag.KichCoList = new SelectList(kichCoList, "MaKichCo", "KichCo1");
            ViewBag.ThuongHieuList = new SelectList(thuongHieuList, "MaThuongHieu", "TenThuongHieu");

            return View();
        }
        [HttpPost]
        public ActionResult Create([Bind(Include = "TenSP,GiaSP,MoTA,SoLuong,MaMauSac,MaKichCo,MaThuongHieu")] SanPham sanPham, HttpPostedFileBase fileUpload)
        {
            if (ModelState.IsValid)
            {
                // Lưu thông tin sản phẩm trước khi thêm hình ảnh để có MaSP
                db.SanPhams.Add(sanPham);
                db.SaveChanges();

                if (fileUpload != null && fileUpload.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("/Content/images/"), fileName);
                    fileUpload.SaveAs(path);

                    var sanPhamAnh = new SanPham_Anh()
                    {
                        MaSP = sanPham.MaSP,
                        Anh = "/Content/images/" + fileName
                    };
                    db.SanPham_Anh.Add(sanPhamAnh);
                    db.SaveChanges();
                }

                // Chuyển hướng người dùng về trang index sau khi thêm sản phẩm thành công
                return RedirectToAction("Index");
            }

            // Nếu ModelState không hợp lệ, hiển thị lại form với thông tin đã nhập
            ViewBag.MauSacList = new SelectList(db.MauSacs, "MaMauSac", "TenMauSac", sanPham.MaMauSac);
            ViewBag.KichCoList = new SelectList(db.KichCoes, "MaKichCo", "KichCo", sanPham.MaKichCo);
            ViewBag.ThuongHieuList = new SelectList(db.ThuongHieux, "MaThuongHieu", "TenThuongHieu", sanPham.MaThuongHieu);
            return View(sanPham);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(int id)
        {
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            var sanPhamAnhList = db.SanPham_Anh.Where(a => a.MaSP == id).ToList();
            foreach (var sanPhamAnh in sanPhamAnhList)
            {
                db.SanPham_Anh.Remove(sanPhamAnh);
            }
            db.SanPhams.Remove(sanPham);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }

            ViewBag.MauSacList = new SelectList(db.MauSacs, "MaMauSac", "TenMauSac", sanPham.MaMauSac);
            ViewBag.KichCoList = new SelectList(db.KichCoes, "MaKichCo", "KichCo1", sanPham.MaKichCo);
            ViewBag.ThuongHieuList = new SelectList(db.ThuongHieux, "MaThuongHieu", "TenThuongHieu", sanPham.MaThuongHieu);

            return View(sanPham);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaSP,TenSP,GiaSP,MoTa,SoLuong,MaMauSac,MaKichCo,MaThuongHieu")] SanPham sanPham, HttpPostedFileBase fileUpload)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sanPham).State = EntityState.Modified;
                db.SaveChanges();

                if (fileUpload != null && fileUpload.ContentLength > 0)
                {
                    // Xóa ảnh cũ nếu có
                    var oldImages = db.SanPham_Anh.Where(a => a.MaSP == sanPham.MaSP);
                    foreach (var oldImage in oldImages)
                    {
                        var oldPath = Server.MapPath(oldImage.Anh);
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                        db.SanPham_Anh.Remove(oldImage);
                    }
                    db.SaveChanges();
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("/Content/images/"), fileName);
                    fileUpload.SaveAs(path);

                    var sanPhamAnh = new SanPham_Anh()
                    {
                        MaSP = sanPham.MaSP,
                        Anh = "/Content/images/" + fileName
                    };
                    db.SanPham_Anh.Add(sanPhamAnh);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            ViewBag.MauSacList = new SelectList(db.MauSacs, "MaMauSac", "TenMauSac", sanPham.MaMauSac);
            ViewBag.KichCoList = new SelectList(db.KichCoes, "MaKichCo", "KichCo1", sanPham.MaKichCo);
            ViewBag.ThuongHieuList = new SelectList(db.ThuongHieux, "MaThuongHieu", "TenThuongHieu", sanPham.MaThuongHieu);

            return View(sanPham);
        }
      
    }
}