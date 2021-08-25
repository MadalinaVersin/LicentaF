using ForAnimalsApplication.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ForAnimalsApplication.Controllers
{
    public class VideoReviewController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: VideoReview
        public ActionResult Index()
        {
            List<VideoReview> reviews = db.VideoReviews.ToList();
            ViewBag.VideoReviews = reviews;
            return View();
        }
        [Authorize(Roles = "User")]
        [HttpGet]
        public ActionResult New(int? id)
        {
            
                VideoReview review = new VideoReview();
                review.NoteList = GetAllNotes();
                review.VideoCompetitorId = (int)id;
                return View(review);
           
        }

        [HttpPost]
        public ActionResult New(VideoReview reviewReq)
        {
            reviewReq.NoteList = GetAllNotes();
            try
            {

                if (ModelState.IsValid)
                {
                    reviewReq.ApplicationUserID = User.Identity.GetUserId();
                    db.VideoReviews.Add(reviewReq);
                    db.SaveChanges();
                    return RedirectToAction("Details", "VideoCompetitor", new { id =reviewReq.VideoCompetitorId });
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
                VideoReview review = db.VideoReviews.Find(id);
                if (review != null)
                {
                    return View(review);
                }

                return HttpNotFound("Nu se poate gasi recenzia cu id-ul " + id.ToString() + " !");
            }

            return HttpNotFound("Lipseste id-ul recenziei!");
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {

            if (id.HasValue)
            {
                VideoReview review = db.VideoReviews.Find(id);
                review.NoteList = GetAllNotes();

                if (review == null)
                {
                    return HttpNotFound("Nu se poate gasi recenzia cu id-ul " + id.ToString() + "!");
                }
                return View(review);
            }
            return HttpNotFound("Lipseste id-ul recenziei!");
        }

        [HttpPut]
        public ActionResult Edit(int id, VideoReview reviewReq)
        {
            reviewReq.NoteList = GetAllNotes();

            VideoReview review = db.VideoReviews.Find(id);
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
                    return RedirectToAction("Details", "VideoCompetitor", new { id = reviewReq.VideoCompetitorId });
                }
                return View(reviewReq);
            }
            catch (Exception)
            {
                return View(reviewReq);
            }
        }


        [HttpDelete]
        public ActionResult Delete(int? id)
        {
            if (id.HasValue)
            {
                VideoReview review = db.VideoReviews.Find(id);
                if (review != null)
                {
                    db.VideoReviews.Remove(review);
                    db.SaveChanges();
                    return RedirectToAction("Details", "VideoCompetitor", new { id = review.VideoCompetitorId });
                }
                return HttpNotFound("Nu se poate gasi recenzia cu id-ul: " + id.ToString() + "!");
            }
            return HttpNotFound("Id-ul recenziei lipseste!");
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