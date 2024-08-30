using DoAn_DAPM.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace DoAn_DAPM.Controllers
{
    public class HomeController : Controller
    {
        private QlyGiayEntities db = new QlyGiayEntities();
        public ActionResult Index(int? page)
        {
            var sanPhams = db.SanPhams.ToList();

            var sanPhamNoiBat = db.SanPhams.Where(sp => sp.MaThuongHieu == 1).Take(4).ToList();

            int pageSize = 8;
            var pageNumber = page ?? 1;
            var onePageOfProducts = sanPhams.ToPagedList(pageNumber, pageSize);

            ViewBag.SanPhamNoiBat = sanPhamNoiBat;
            return View(onePageOfProducts);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TimKiem(string searchInput)
        {
            var ketQuaTimKiem = db.SanPhams.Where(p => p.TenSP.Contains(searchInput)).ToList();

            return PartialView("_KetQuaTimKiem", ketQuaTimKiem);
        }
    }
}