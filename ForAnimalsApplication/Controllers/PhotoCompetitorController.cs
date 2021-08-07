using ForAnimalsApplication.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ForAnimalsApplication.Controllers
{
    public class PhotoCompetitorController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: PhotoCompetitor
        public ActionResult Index()
        {
            List<PhotoCompetitor> competitors = db.PhotoCompetitors.ToList();
            ViewBag.PhotoCompetitors = competitors;
            return View();
        }

        [HttpGet]
        public ActionResult New(int? id)
        {
            if (id.HasValue)
            {
                PhotoCompetitor competitor = new PhotoCompetitor();
                competitor.AgeList = GetAllAges();
                competitor.GenderList = GetAllGenders();
                competitor.CompetitionId = (int)id;
                return View(competitor);
            }
            return HttpNotFound("Missing animal id parameter!");
        }

        [HttpPost]
        public ActionResult New(PhotoCompetitor competitorReq)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Salvam imaginea cu animalul
                    string fileName = Path.GetFileNameWithoutExtension(competitorReq.ImageFile.FileName);
                    string extension = Path.GetExtension(competitorReq.ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    competitorReq.ImagePath = "~/Images/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    competitorReq.ImageFile.SaveAs(fileName);

                    //adaugam user-ul care a pus concurentul
                    competitorReq.ApplicationUserID = User.Identity.GetUserId();
                    db.PhotoCompetitors.Add(competitorReq);
                    db.SaveChanges();
                    return RedirectToAction("Details", "Competition", new { id = competitorReq.CompetitionId });

                }
                competitorReq.AgeList = GetAllAges();
                competitorReq.GenderList = GetAllGenders();
                
                return View(competitorReq);
            }
            catch (Exception e)
            {
                var msg = e.Message;
                return View(competitorReq);
            }

        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                PhotoCompetitor competitor = db.PhotoCompetitors.Find(id);
                if (competitor == null)
                {
                    return HttpNotFound("Couldn't find the competitor with id " + id.ToString() + "!");
                }
                return View(competitor);
            }
            return HttpNotFound("Missing competitor id parameter!");
        }

        [HttpPut]
        public ActionResult Edit(int id, PhotoCompetitor competitorReq)
        {

            // preluam competitorul  pe care vrem sa o modificam din baza de date
            PhotoCompetitor competitor = db.PhotoCompetitors.Find(id);

            try
            {
                int updateImg = 0;
                if (competitorReq.ImageFile != null)
                {
                    updateImg = 1;
                    string fileName = Path.GetFileNameWithoutExtension(competitorReq.ImageFile.FileName);
                    string extension = Path.GetExtension(competitorReq.ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    competitorReq.ImagePath = "~/Images/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    competitorReq.ImageFile.SaveAs(fileName);

                }




                if (ModelState.IsValid)
                {
                    if (TryUpdateModel(competitor))
                    {
                        competitor.Name = competitorReq.Name;
                        competitor.Description = competitorReq.Description;
                        competitor.ImagePath = competitorReq.ImagePath;
                        if (updateImg == 1)
                        {
                            competitor.ImageFile = competitorReq.ImageFile;
                            competitor.ImagePath = competitorReq.ImagePath;
                        }
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                return View(competitorReq);
            }
            catch (Exception)
            {
                return View(competitorReq);
            }
        }

        public ActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                PhotoCompetitor competitor = db.PhotoCompetitors.Find(id);
                if (competitor != null)
                {
                    //ViewBag.NewReview = IsUserReview((int)id, User.Identity.GetUserId());
                    ViewBag.PhotoReviews = db.PhotoReviews.Include("ApplicationUser").Where(u => u.PhotoCompetitorId == id);
                    return View(competitor);
                }
                return HttpNotFound("Couldn't find the animal with id " + id.ToString() + "!");
            }
            return HttpNotFound("Missing animal id parameter!");
        }

        public Boolean IsUserReview(int competitorId, string userId)
        {
            var userReview = db.PhotoReviews.ToList().Where(u => u.PhotoCompetitorId == competitorId && u.ApplicationUserID == userId);
            if(userReview.Count() == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public IEnumerable<SelectListItem> GetAllAges()
        {
            var selectList = new List<SelectListItem>();

            for (var i = 0; i < 20; i++)
            {
                selectList.Add(new SelectListItem
                {
                    Value = i.ToString() + "-" + (i + 1).ToString() + " ani",
                    Text = i.ToString() + "-" + (i + 1).ToString() + " ani"
                });
            }
            return selectList;
        }

        public IEnumerable<SelectListItem> GetAllGenders()
        {
            var selectList = new List<SelectListItem>();
            selectList.Add(new SelectListItem
            {
                Value = "Femela",
                Text = "Femela"
             });

            selectList.Add(new SelectListItem
            {
                Value = "Mascul",
                Text = "Mascul"
            });
            return selectList;
        }

    }
}