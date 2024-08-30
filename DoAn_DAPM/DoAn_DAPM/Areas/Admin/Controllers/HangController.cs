using DoAn_DAPM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn_DAPM.Areas.Admin.Controllers
{
    public class HangController : Controller
    {
        QlyGiayEntities db = new QlyGiayEntities();
        // GET: Admin/Hang
        public ActionResult Index()
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Admin");
            return View(db.ThuongHieux.ToList());
        }
    }
}