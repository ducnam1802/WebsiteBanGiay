using DoAn_DAPM.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn_DAPM.Controllers
{
    public class GioHangController : Controller
    {
        private QlyGiayEntities db = new QlyGiayEntities();
        // GET: GioHang
        public List<Cart> LayGioHang()
        {
            List<Cart> giohang = Session["GioHang"] as List<Cart>;
            if (giohang == null)
            {
                giohang = new List<Cart>();
                Session["GioHang"] = giohang;
            }
            return giohang;
            
        }
        public ActionResult ThemSanPhamVaoGio(int MaSP)
        {
            List<Cart> gioHang = LayGioHang();

            Cart sanPham = gioHang.FirstOrDefault(s => s.MaSp == MaSP);
            if (sanPham == null)
            {
                sanPham = new Cart(MaSP);
                gioHang.Add(sanPham);
            }
            else
            {
                sanPham.Soluong++;
            }
            return RedirectToAction("Details", "Sanpham", new {idx = MaSP});
        }
        private int TinhTongSL()
        {
            int tongSL = 0;
            List<Cart> gioHang = LayGioHang();
            if (gioHang != null)
                tongSL = gioHang.Sum(sp => sp.Soluong);
            return tongSL;
        }
        private double TinhTongTien()
        {
            double TongTien = 0;
            List<Cart> gioHang = LayGioHang();
            if (gioHang != null)
                TongTien = gioHang.Sum(sp => sp.ThanhTien);
            return TongTien;
        }
        public ActionResult HienThiGioHang()
        {
            List<Cart> gioHang = LayGioHang();

            if(gioHang == null || gioHang.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.TongSL = TinhTongSL();
            ViewBag.TongTien = TinhTongTien();
            return View(gioHang);
        }
        public ActionResult GioHangPartial()
        {
            ViewBag.TongSL = TinhTongSL();
            ViewBag.TongTien = TinhTongTien();
            return PartialView();
        }
        public ActionResult SuaSoLuong(int MaSP, int soluongmoi)
        {
            List<Cart> giohang = Session["GioHang"] as List<Cart>;
            Cart itemSua = giohang.FirstOrDefault(m => m.MaSp == MaSP);
            if (itemSua != null)
            {
                if (soluongmoi > itemSua.soLuongTrongKho)
                {
                    soluongmoi = itemSua.soLuongTrongKho; 
                }
                itemSua.Soluong = soluongmoi;
            }
            return RedirectToAction("HienThiGioHang");
        }
        public ActionResult XoaKhoiGio(int MaSP)
        {
            List<Cart> giohang = Session["GioHang"] as List<Cart>;
            Cart itemXoa = giohang.FirstOrDefault(m => m.MaSp == MaSP);
            if (itemXoa != null)
            {
                giohang.Remove(itemXoa);
            }
            return RedirectToAction("HienThiGioHang");
        }
        public ActionResult DatHang()
        {
            if (Session["KH"] == null)
                return RedirectToAction("Login", "Account");
            List<Cart> gioHang = LayGioHang();
            if(gioHang == null || gioHang.Count == 0)
                return RedirectToAction("Index", "Home");

            ViewBag.TongSL = TinhTongSL();
            ViewBag.TongTien = TinhTongTien();
            return View(gioHang);
        }
        
        public ActionResult DongYDatHang()
        {
            NguoiDung nd = Session["KH"] as NguoiDung;
            List<Cart> gioHang = LayGioHang();

            DonHang donHang = new DonHang();
            donHang.MaNguoiDung = nd.MaNguoiDung;
            donHang.TenNguoiNhan = nd.TenNguoiDung;
            donHang.NgayXuatDon = DateTime.Now;
            donHang.NgayDuKien = DateTime.Now.AddDays(2);
            donHang.DiaChi = nd.DiaChi;
            donHang.SDT = nd.SDT;
            donHang.TongTien = (decimal)TinhTongTien();
            donHang.TinhTrang = "Đang xác nhận";

            db.DonHangs.Add(donHang);
            db.SaveChanges();

            foreach (var sanpham in gioHang)
            {
                ChiTietDonHang chitiet = new ChiTietDonHang();
                chitiet.MaDonHang = donHang.MaDonHang;
                chitiet.MaSP = sanpham.MaSp;
                chitiet.SoLuong = sanpham.Soluong;
                chitiet.Gia = donHang.TongTien;
                db.ChiTietDonHangs.Add(chitiet);
            }
            db.SaveChanges();

            Session["GioHang"] = null;
            return RedirectToAction("HoanThanhDonHang");
        }
        public ActionResult HoanThanhDonHang()
        {
            return View();
        }
    }
}