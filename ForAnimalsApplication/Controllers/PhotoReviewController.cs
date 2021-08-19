﻿using ForAnimalsApplication.Models;
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
            if (IsUserReview((int)id, User.Identity.GetUserId()) == false)
            { 
                PhotoReview review = new PhotoReview();
                review.NoteList = GetAllNotes();
                review.PhotoCompetitorId = (int)id;
                return View(review);
            }
            else
            {
                return RedirectToAction("Details", "PhotoCompetitor", new { id = id });
            }
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

                    PhotoCompetitor competitor = db.PhotoCompetitors.Find(reviewReq.PhotoCompetitorId);
                    competitor.FinalNote = CalculateFinalNote(reviewReq.PhotoCompetitorId);
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
                    return HttpNotFound("Couldn't find the review with id " + id.ToString() + "!");
                }
                return View(review);
            }
            return HttpNotFound("Couldn't find the review with id " + id.ToString() + "!");
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

                        PhotoCompetitor competitor = db.PhotoCompetitors.Find(reviewReq.PhotoCompetitorId);
                        competitor.FinalNote = CalculateFinalNote(reviewReq.PhotoCompetitorId);
                        db.SaveChanges();
                        return RedirectToAction("Details", "PhotoCompetitor", new { id = competitor.PhotoCompetitorId });
                    }
                    return RedirectToAction("Details", "PhotoCompetitor", new { id = review.PhotoCompetitor.PhotoCompetitorId });
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
                PhotoReview review = db.PhotoReviews.Find(id);
                if (review != null)
                {
                    db.PhotoReviews.Remove(review);
                    db.SaveChanges();
                    PhotoCompetitor competitor = db.PhotoCompetitors.Find(review.PhotoCompetitorId);
                    competitor.FinalNote = CalculateFinalNote(review.PhotoCompetitorId);
                    db.SaveChanges();
                    return RedirectToAction("Details", "PhotoCompetitor", new { id = competitor.PhotoCompetitorId });
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
    }
}