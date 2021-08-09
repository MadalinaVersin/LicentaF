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

                    VideoCompetitor competitor = db.VideoCompetitors.Find(reviewReq.VideoCompetitorId);
                    competitor.FinalNote = CalculateFinalNote(reviewReq.VideoCompetitorId);
                    db.SaveChanges();
                    return RedirectToAction("Details", "VideoCompetitor", new { id = competitor.VideoCompetitorId });
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

                return HttpNotFound("Couldn't find the review you are searching for...");
            }

            return HttpNotFound("Parameter is missing...");
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
                    return HttpNotFound("Couldn't find the review with id " + id.ToString() + "!");
                }
                return View(review);
            }
            return HttpNotFound("Couldn't find the review with id " + id.ToString() + "!");
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

                        VideoCompetitor competitor = db.VideoCompetitors.Find(reviewReq.VideoCompetitorId);
                        competitor.FinalNote = CalculateFinalNote(reviewReq.VideoCompetitorId);
                        db.SaveChanges();
                        return RedirectToAction("Details", "VideoCompetitor", new { id = competitor.VideoCompetitorId });
                    }
                    return RedirectToAction("Details", "VideoCompetitor", new { id = reviewReq.VideoCompetitor.VideoCompetitorId });
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
                    VideoCompetitor competitor = db.VideoCompetitors.Find(review.VideoCompetitorId);
                    competitor.FinalNote = CalculateFinalNote(review.VideoCompetitorId);
                    db.SaveChanges();
                    return RedirectToAction("Details", "VideoCompetitor", new { id = competitor.VideoCompetitorId });
                }
                return HttpNotFound("Nu se poate gasi comentariul cu id-ul: " + id.ToString() + "!");
            }
            return HttpNotFound("Id-ul comentariului lipseste!");
        }


        public IEnumerable<SelectListItem> GetAllNotes()
        {
            var selectList = new List<SelectListItem>();

            for (var i = 0; i < 5; i++)
            {
                selectList.Add(new SelectListItem
                {
                    Value = i.ToString(),
                    Text = i.ToString()
                });
            }
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