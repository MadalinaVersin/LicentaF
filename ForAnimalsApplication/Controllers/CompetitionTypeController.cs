using ForAnimalsApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ForAnimalsApplication.Controllers
{
    public class CompetitionTypeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: CompetitionType
        public ActionResult Index()
        {
            ViewBag.CompetitionTypes = db.CompetitionTypes.ToList();
            return View();
        }

        [HttpGet]
        public ActionResult New()
        {
            CompetitionType competitionType = new CompetitionType();
            return View(competitionType);
        }

        [HttpPost]
        public ActionResult New(CompetitionType compTypeReq)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.CompetitionTypes.Add(compTypeReq);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                return View(compTypeReq);
            }
            catch (Exception e)
            {
                return View(compTypeReq);
            }
        }
    }
}