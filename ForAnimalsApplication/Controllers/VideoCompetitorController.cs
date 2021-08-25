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
        [Authorize(Roles = "User, Block")]
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
            return HttpNotFound("Lipseste id-ul competitorului!");
        }

        [HttpPost]
        public ActionResult New(VideoCompetitor competitorReq)
        {
            competitorReq.AgeList = GetAllAges();
            competitorReq.GenderList = GetAllGenders();
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
                competitor.AgeList = GetAllAges();
                competitor.GenderList = GetAllGenders();
                if (competitor == null)
                {
                    return HttpNotFound("Nu poate fi gasit competitorul cu id-ul" + id.ToString() + "!");
                }
                return View(competitor);
            }
            return HttpNotFound("Lipseste id-ul competitorului!");
        }

        [HttpPut]
        public ActionResult Edit(int id, VideoCompetitor competitorReq)
        {
            competitorReq.AgeList = GetAllAges();
            competitorReq.GenderList = GetAllGenders();
            // preluam competitorul  pe care vrem sa il modificam din baza de date
            VideoCompetitor competitor = db.VideoCompetitors.Find(id);

            try
            {
                int updateVideo = 0;
                if (competitorReq.VideoFile != null)
                {
                    updateVideo = 1;

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
                        competitor.MicrochipNumber = competitorReq.MicrochipNumber;
                        competitor.Age = competitorReq.Age;
                        competitor.Gender = competitorReq.Gender;
                        competitor.Description = competitorReq.Description;
                        if (updateVideo == 1)
                        {
                            competitor.Vname = competitorReq.Vname;
                            competitor.Vpath = competitorReq.Vpath;
                        }
                        db.SaveChanges();
                    }
                    return RedirectToAction("Details", "Competition", new { id = competitorReq.CompetitionId });
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
                ViewBag.Competition = db.Competitions.Find(competitor.CompetitionId);
                if (competitor != null)
                {
                    ViewBag.ReviewedOrOwner = IsUserReviewOrOwner((int)id, competitor.ApplicationUserID);
                    if (IsUserReviewOrOwner((int)id, competitor.ApplicationUserID) == true)
                    {
                        if (competitor.ApplicationUser.Id == User.Identity.GetUserId())
                        {
                            ViewBag.MessageOwner = "Sunteti stapanul acestui animal.";
                        }
                       /* else
                        {
                            ViewBag.Message = "Ati dat deja o recenzie pentru acest competitor!";
                        }*/
                    }
                    ViewBag.VideoReviews = db.VideoReviews.Include("ApplicationUser").Where(u => u.VideoCompetitorId == id);
                    return View(competitor);
                }
                return HttpNotFound("Nu poate fi gasit competitorul cu id-ul " + id.ToString() + "!");
            }
            return HttpNotFound("Id-ul competitorului lipseste!");
        }

        [HttpDelete]
        public ActionResult Delete(int? id)
        {
            if (id.HasValue)
            {
                VideoCompetitor videoCompetitor = db.VideoCompetitors.Find(id);
                if (videoCompetitor != null)
                {
                    db.VideoCompetitors.Remove(videoCompetitor);
                    db.SaveChanges();
                    return RedirectToAction("Details", "Competition", new { id = videoCompetitor.CompetitionId });
                }
                return HttpNotFound("Nu se poate gasi competitorul cu id-ul:" + id.ToString());
            }
            return HttpNotFound("Id-ul competitorului lipseste!");
        }

        [HttpGet]
        public ActionResult GiveJuryNote(int? id)
        {
            if (id.HasValue)
            {
                VideoCompetitor competitor = db.VideoCompetitors.Find(id);
                competitor.AgeList = GetAllAges();
                competitor.GenderList = GetAllGenders();

                if (competitor == null)
                {
                    return HttpNotFound("Nu se poate gasi competitorul cu id-ul" + id.ToString() + "!");
                }
                return View(competitor);
            }
            return HttpNotFound("Lipseste id-ul competitorului!");
        }
        [HttpPut]
        public ActionResult GiveJuryNote(int id, VideoCompetitor competitorReq)
        {
            competitorReq.AgeList = GetAllAges();
            competitorReq.GenderList = GetAllGenders();
            // preluam competitorul  pe care vrem sa o modificam din baza de date
            VideoCompetitor competitor = db.VideoCompetitors.Find(id);

            try
            {
                if (ModelState.IsValid)
                {
                    if (competitorReq.JuryNote <= 0 || competitorReq.JuryNote > 5)
                    {
                        ViewBag.Message = "Nota trebuie sa fie mai mare ca 0 si mai mica sau egla cu 5!";
                        return View(competitorReq);
                    }


                    if (TryUpdateModel(competitor))
                    {
                        competitor.JuryNote = competitorReq.JuryNote;
                        db.SaveChanges();
                        competitor.FinalNote = CalculateFinalNote(id);
                        db.SaveChanges();
                    }
                    return RedirectToAction("Details", "VideoCompetitor", new { id = id });

                }
                return View(competitorReq);
            }
            catch (Exception)
            {
                return View(competitorReq);
            }
        }


        public Boolean IsUserReviewOrOwner(int competitorId, string userId)
        {
            var userReview = db.VideoReviews.ToList().Where(u => u.VideoCompetitorId == competitorId && u.ApplicationUserID == User.Identity.GetUserId());
            if (userReview.Count() == 0 && userId != User.Identity.GetUserId())
            {
                return false;
            }
            else
            {
                return true;
            }

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

        public double CalculateFinalNote(int competitorId)
        {
            List<VideoReview> reviews = db.VideoReviews.Where(a => a.VideoCompetitorId == competitorId).ToList();
            VideoCompetitor competitor = db.VideoCompetitors.Find(competitorId);
            double sum = 0;
            for (var i = 0; i < reviews.Count; i++)
            {
                sum = sum + reviews[i].Note;
            }
            if (reviews.Count != 0)
            {
                sum = sum / reviews.Count;
            }
            sum = sum + competitor.JuryNote;
            sum = sum / 2;
            return sum;


        }

    }
}