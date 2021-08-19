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

        [Authorize(Roles = "User, Block")]
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
            return HttpNotFound("Lipseste id-ul competitorului!");
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
                competitorReq.AgeList = GetAllAges();
                competitorReq.GenderList = GetAllGenders();
                return View(competitorReq);
            }

        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                PhotoCompetitor competitor = db.PhotoCompetitors.Find(id);
                competitor.AgeList = GetAllAges();
                competitor.GenderList = GetAllGenders();

                if (competitor == null)
                {
                    return HttpNotFound("Nu se poate gasi competitorul cu id-ul " + id.ToString() + "!");
                }
                return View(competitor);
            }
            return HttpNotFound("Lipseste id-ul competitorului!");
        }

        [HttpPut]
        public ActionResult Edit(int id, PhotoCompetitor competitorReq)
        {
            competitorReq.AgeList = GetAllAges();
            competitorReq.GenderList = GetAllGenders();
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
                        competitor.MicrochipNumber = competitorReq.MicrochipNumber;
                        competitor.Description = competitorReq.Description;
                        competitor.Age = competitorReq.Age;
                        competitor.Gender = competitorReq.Gender;
                        competitor.ImagePath = competitorReq.ImagePath;
                        if (updateImg == 1)
                        {
                            competitor.ImageFile = competitorReq.ImageFile;
                            competitor.ImagePath = competitorReq.ImagePath;
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
                PhotoCompetitor competitor = db.PhotoCompetitors.Find(id);
                if (competitor != null)
                {
                    ViewBag.ReviewdOrOwner = IsUserReview((int)id, User.Identity.GetUserId());
                    if(IsUserReview((int)id, User.Identity.GetUserId()) == true)
                    {
                        if(competitor.ApplicationUser.Id == User.Identity.GetUserId())
                        {
                            ViewBag.MessageOwner = "Sunteti stapanul acestui animal.";
                        }/* else {
                            ViewBag.Message = "Ati dat deja o recenzie pentru acest competitor!";
                        }*/
                    }
                    ViewBag.PhotoReviews = db.PhotoReviews.Include("ApplicationUser").Where(u => u.PhotoCompetitorId == id);
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
                PhotoCompetitor photoCompetitor = db.PhotoCompetitors.Find(id);
                if (photoCompetitor != null)
                {
                    db.PhotoCompetitors.Remove(photoCompetitor);
                    db.SaveChanges();
                    return RedirectToAction("Index");
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
                PhotoCompetitor competitor = db.PhotoCompetitors.Find(id);
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
        public ActionResult GiveJuryNote(int id, PhotoCompetitor competitorReq)
        {
            competitorReq.AgeList = GetAllAges();
            competitorReq.GenderList = GetAllGenders();
            // preluam competitorul  pe care vrem sa o modificam din baza de date
            PhotoCompetitor competitor = db.PhotoCompetitors.Find(id);

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
                        return RedirectToAction("Index");
                    }

                }
                return View(competitorReq);
            }
            catch (Exception)
            {
                return View(competitorReq);
            }
        }

        public Boolean IsUserReview(int competitorId, string userId)
        {
            var userReview = db.PhotoReviews.ToList().Where(u => u.PhotoCompetitorId == competitorId && u.ApplicationUserID == userId);
            PhotoCompetitor competitor = db.PhotoCompetitors.Find(competitorId);
            if (userReview.Count() == 0 && competitor.ApplicationUserID != User.Identity.GetUserId())
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        public double CalculateFinalNote(int competitorId)
        {
            List<PhotoReview> reviews = db.PhotoReviews.Where(a => a.PhotoCompetitorId == competitorId).ToList();
            PhotoCompetitor competitor = db.PhotoCompetitors.Find(competitorId);
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