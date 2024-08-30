using DoAn_DAPM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn_DAPM.Controllers
{
    public class SanphamController : Controller
    {
        private QlyGiayEntities db = new QlyGiayEntities();
        // GET: Sanpham
        public ActionResult Index()
        {
            return View();  
        }
        public ActionResult Details(int idx)
        {
            var sanpham = db.SanPhams.Include("SanPham_Anh").FirstOrDefault(s => s.MaSP == idx);
            if (sanpham == null)
            {
                return HttpNotFound();
            }
            var relatedProducts = db.SanPhams.Where(p => p.MaThuongHieu == sanpham.MaThuongHieu && p.MaSP != sanpham.MaSP).ToList();
            ViewBag.RelatedProducts = relatedProducts;
            return View(sanpham);
        }
        public ActionResult TheoThuongHieu(int MaThuongHieu)
        {
            var sanPhamsTheoTH = db.SanPhams.Where(s => s.MaThuongHieu == MaThuongHieu).ToList();
            var thuongHieu = db.ThuongHieux.Find(MaThuongHieu);
            if (thuongHieu == null)
            {
                return HttpNotFound();
            }
            ViewBag.TenThuongHieu = thuongHieu.TenThuongHieu;
            return View(sanPhamsTheoTH);    
        }
    }
}