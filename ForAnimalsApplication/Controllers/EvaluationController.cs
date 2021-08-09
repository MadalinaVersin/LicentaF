using ForAnimalsApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ForAnimalsApplication.Controllers
{
    public class EvaluationController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext(); 
        // GET: Evaluation
        public ActionResult Index()
        {
            ViewBag.Competitions = db.Competitions.ToList().Where(u => u.EndDate < DateTime.Now && u.Evaluated == false);  
            return View();
        }

        public ActionResult Evaluates(int id)
        {
            Competition competition = db.Competitions.Include("CompetitionType").Where(u => u.CompetitionId == id).FirstOrDefault();
            if (competition.CompetitionType.Name == "Photo")
            {
                var notEvaluatesF = db.PhotoCompetitors.ToList().Where(u => u.CompetitionId == id && u.JuryNote == 0);
                if(notEvaluatesF.Count() != 0)
                {
                    ViewBag.Message = "Trebuie sa evaluati toti concurentii!";
                    return View();
                } else
                {
                    CalculateWinner(id);
                    ViewBag.Message = "Evaluare realizata cu succes!";
                    competition.Evaluated = true;
                    db.SaveChanges();
                    return View();
                }
               
            }
            else
            {
                
                var notEvaluatesV = db.VideoCompetitors.ToList().Where(u => u.CompetitionId == id && u.JuryNote == 0);
                if (notEvaluatesV.Count() != 0)
                {
                    ViewBag.Message = "Trebuie sa evaluati toti concurentii!";
                    return View();
                }
                else
                {
                    ViewBag.Message = "Evaluare realizata cu succes!";
                    return View();
                }
            }
           
        }

        public void CalculateWinner(int id)
        {
            List<PhotoCompetitor> photoCompetitors = db.PhotoCompetitors.Where(u => u.CompetitionId == id).ToList();
            double maxNote = photoCompetitors[0].FinalNote;
            for(var i = 0; i < photoCompetitors.Count(); i++)
            {
                if(photoCompetitors[i].FinalNote > maxNote)
                {
                    maxNote = photoCompetitors[i].FinalNote;
                }
            }

            List<PhotoCompetitor> winners = db.PhotoCompetitors.Where(u => u.FinalNote == maxNote).ToList();
            for(var i = 0; i< winners.Count(); i++)
            {
                winners[i].Winner = true;
                db.SaveChanges();
            }

        }

        
    }
}