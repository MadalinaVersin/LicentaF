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
    public class VideoCompetitorController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: VideoCompetitor
        public ActionResult Index()
        {
            List<VideoCompetitor> competitors = db.VideoCompetitors.ToList();
            ViewBag.VideoCompetitors = competitors;
            return View();
        }

        [HttpGet]
        public ActionResult New(int? id)
        {
            if (id.HasValue)
            {
                VideoCompetitor competitor = new VideoCompetitor();
                competitor.AgeList = GetAllAges();
                competitor.GenderList = GetAllGenders();
                competitor.CompetitionId = (int)id;
                return View(competitor);
            }
            return HttpNotFound("Missing animal id parameter!");
        }

        [HttpPost]
        public ActionResult New(VideoCompetitor competitorReq)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (competitorReq.VideoFile != null)
                    {
                        string filename = Path.GetFileNameWithoutExtension(competitorReq.VideoFile.FileName);
                        if (competitorReq.VideoFile.ContentLength < 104857600)
                        {
                            string extension = Path.GetExtension(competitorReq.VideoFile.FileName);
                            filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
                            competitorReq.VideoFile.SaveAs(Server.MapPath("/Videofiles/" + filename));
                            competitorReq.Vname = filename;
                            competitorReq.Vpath = "/Videofiles/" + filename;

                        }
                    }
                    //adaugam user-ul care a pus concurentul
                    competitorReq.ApplicationUserID = User.Identity.GetUserId();
                    db.VideoCompetitors.Add(competitorReq);
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
                VideoCompetitor competitor = db.VideoCompetitors.Find(id);
                if (competitor == null)
                {
                    return HttpNotFound("Couldn't find the competitor with id " + id.ToString() + "!");
                }
                return View(competitor);
            }
            return HttpNotFound("Missing competitor id parameter!");
        }

        [HttpPut]
        public ActionResult Edit(int id, VideoCompetitor competitorReq)
        {

            // preluam competitorul  pe care vrem sa il modificam din baza de date
            VideoCompetitor competitor = db.VideoCompetitors.Find(id);

            try
            {
                int updateImg = 0;
                if (competitorReq.VideoFile != null)
                {
                    updateImg = 1;

                    string filename = Path.GetFileNameWithoutExtension(competitorReq.VideoFile.FileName);
                    if (competitorReq.VideoFile.ContentLength < 1048576000)
                    {
                        string extension = Path.GetExtension(competitorReq.VideoFile.FileName);
                        filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
                        competitorReq.VideoFile.SaveAs(Server.MapPath("/Videofiles/" + filename));
                        competitorReq.Vname = filename;
                        competitorReq.Vpath = "/Videofiles/" + filename;

                    }

                }

                if (ModelState.IsValid)
                {
                    if (TryUpdateModel(competitor))
                    {
                        competitor.Name = competitorReq.Name;
                        competitor.Description = competitorReq.Description;
                        competitor.Vpath = competitorReq.Vpath;
                        if (updateImg == 1)
                        {
                            competitor.Vname = competitorReq.Vname;
                            competitor.Vpath = competitorReq.Vpath;
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
                VideoCompetitor competitor = db.VideoCompetitors.Find(id);
                if (competitor != null)
                {
                    ViewBag.VideoReviews = db.VideoReviews.Include("ApplicationUser").Where(u => u.VideoCompetitorId == id);
                    return View(competitor);
                }
                return HttpNotFound("Couldn't find the animal with id " + id.ToString() + "!");
            }
            return HttpNotFound("Missing animal id parameter!");
        }

        public IEnumerable<SelectListItem> GetAllAges()
        {
            var selectList = new List<SelectListItem>();

            for (var i = 0; i < 20; i++)
            {
                selectList.Add(new SelectListItem
                {
                    Value = i.ToString() +"-" +(i+1).ToString()+ " ani",
                    Text = i.ToString() + "-" + (i + 1).ToString()+" ani"
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