using ForAnimalsApplication.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ForAnimalsApplication.Controllers
{
    public class PhotoReviewController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: PhotoReview
        public ActionResult Index()
        {
            List<PhotoReview> reviews = db.PhotoReviews.ToList();
            ViewBag.PhotoReviews = reviews;
            return View();
        }
        [Authorize(Roles = "User")]
        [HttpGet]
        public ActionResult New(int? id)
        {
          
                PhotoReview review = new PhotoReview();
                review.NoteList = GetAllNotes();
                review.PhotoCompetitorId = (int)id;
                return View(review);
            
        }

        [HttpPost]
        public ActionResult New(PhotoReview reviewReq)
        {
            reviewReq.NoteList = GetAllNotes();
            try
            {

                if (ModelState.IsValid)
                {
                    reviewReq.ApplicationUserID = User.Identity.GetUserId();
                    db.PhotoReviews.Add(reviewReq);
                    db.SaveChanges();
                    return RedirectToAction("Details", "PhotoCompetitor", new { id = reviewReq.PhotoCompetitorId });
                }
                return View(reviewReq);
            }
            catch (Exception e)
            {
                return View(reviewReq);
            }

        }
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                PhotoReview review = db.PhotoReviews.Find(id);
                if (review != null)
                {
                    return View(review);
                }

                return HttpNotFound("Nu se poate gasi recenzia cautata!");
            }

            return HttpNotFound("Lipseste paramestrul!");
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {

            if (id.HasValue)
            {
                PhotoReview review = db.PhotoReviews.Find(id);
                review.NoteList = GetAllNotes();

                if (review == null)
                {
                    return HttpNotFound("Nu se poate gasi recenzia cu id-ul " + id.ToString() + "!");
                }
                return View(review);
            }
            return HttpNotFound("Id-ul recenziei lipseste!");
        }

        [HttpPut]
        public ActionResult Edit(int id, PhotoReview reviewReq)
        {
            reviewReq.NoteList = GetAllNotes();

            PhotoReview review = db.PhotoReviews.Find(id);
            try
            {
                if (ModelState.IsValid)
                {
                    if (TryUpdateModel(review))
                    {
                        review.Text = reviewReq.Text;
                        review.Note = reviewReq.Note;
                        review.ReviewDate = DateTime.Now;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Details", "PhotoCompetitor", new { id = review.PhotoCompetitorId });
                }
                return View(reviewReq);
            }
            catch (Exception e)
            {
                return View(reviewReq);
            }
        }


        [HttpDelete]
        public ActionResult Delete(int? id)
        {
            if (id.HasValue)
            {
                PhotoReview review = db.PhotoReviews.Find(id);
                if (review != null)
                {
                    db.PhotoReviews.Remove(review);
                    db.SaveChanges();
                    return RedirectToAction("Details", "PhotoCompetitor", new { id = review.PhotoCompetitorId });
                }
                return HttpNotFound("Nu se poate gasi comentariul cu id-ul:" + id.ToString() + "!");
            }
            return HttpNotFound("Id-ul comentariului lipseste!");
        }



        public IEnumerable<SelectListItem> GetAllNotes()
        {
            var selectList = new List<SelectListItem>();

            for (var i = 1; i < 6; i++)
            {
                selectList.Add(new SelectListItem
                {
                    Value = i.ToString(),
                    Text = i.ToString()
                });
            }
            return selectList;
        }
    }
}