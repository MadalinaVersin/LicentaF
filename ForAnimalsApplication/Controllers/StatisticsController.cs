using ForAnimalsApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ForAnimalsApplication.Controllers
{
    public class StatisticsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Statistics
        public ActionResult Index()
        {
            return View();
        }
   
        public ActionResult AboutAllCompetitions()
        {
            ViewBag.Competitions = db.Competitions.Include("CompetitionType").ToList();
            return View();
        }
        public JsonResult GetAboutAllCompetitions()
        {
            List<Competition> competitions = db.Competitions.ToList();
            int[] numberOfConcurents = new int[competitions.Count()];
          
            for (var i = 0; i < competitions.Count(); i++)
            {
                if(competitions[i].CompetitionType.Name == "Photo")
                {
                    var photoCompetitors = db.PhotoCompetitors.ToList().Where(u => u.CompetitionId == competitions[i].CompetitionId);
                    numberOfConcurents[i] = photoCompetitors.Count();
                }
                else
                {
                    var videoCompetitors = db.VideoCompetitors.ToList().Where(u => u.CompetitionId == competitions[i].CompetitionId);
                    numberOfConcurents[i] = videoCompetitors.Count();
                }
            }
            List<object> chartData = new List<object>();
            chartData.Add(new object[] { "Nume competitie", "Numar participanti" });
            for (var i = 0; i < competitions.Count(); i++)
            {
                chartData.Add(new object[]
                {competitions[i].CompetitionName, numberOfConcurents[i]});
            }


            return Json(chartData, JsonRequestBehavior.AllowGet);


        }

        public JsonResult GetCompetitionType()
        {
            List<CompetitionType> competitionTypes = db.CompetitionTypes.ToList();
            List<object> chartData = new List<object>();
            chartData.Add(new object[] { "Tip competitie", "Numar competitii de acest tip" });
            for (var i = 0; i < competitionTypes.Count(); i++)
            {
                var competitions = db.Competitions.ToList().Where(u => u.CompetitionTypeId == competitionTypes[i].CompetitionTypeId);
                chartData.Add(new object[] { competitionTypes[i].Name, competitions.Count() });

            }
            return Json(chartData, JsonRequestBehavior.AllowGet);

        }


        public ActionResult AboutCompetitor(string id)
        {
            ViewBag.Microchip = id;
            ViewBag.VideoCompetitors = db.VideoCompetitors.Include("Competition").ToList();
            ViewBag.PhotoCompetitors = db.PhotoCompetitors.Include("Competition").ToList();

            return View();
        }

        public JsonResult GetReviews(string id)
        {
            List<object> chartData = new List<object>();
            chartData.Add(new object[] { "Nume competitie", "Numar review-uri" });
            List<VideoCompetitor> videoCompetitors = db.VideoCompetitors.Include("Competition").ToList();
            for(var i = 0; i < videoCompetitors.Count(); i++)
            {
                if (videoCompetitors[i].MicrochipNumber == id)
                {
                    var reviews = db.VideoReviews.ToList().Where(u => u.VideoCompetitorId == videoCompetitors[i].VideoCompetitorId);
                    chartData.Add(new object[] { videoCompetitors[i].Competition.CompetitionName, reviews.Count() });
                }

            }

            List<PhotoCompetitor> photoCompetitors = db.PhotoCompetitors.Include("Competition").ToList();
            for (var i = 0; i < photoCompetitors.Count(); i++)
            {
                if(photoCompetitors[i].MicrochipNumber == id)
                {
                    var reviews = db.PhotoReviews.ToList().Where(u => u.PhotoCompetitorId == photoCompetitors[i].PhotoCompetitorId);
                    chartData.Add(new object[] { photoCompetitors[i].Competition.CompetitionName, reviews.Count()});
                }
            }
            return Json(chartData, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetWonCompetition(string id)
        {
            List<object> chartData = new List<object>();
            chartData.Add(new object[] { "Rezultate competitii", "Numar" });
            List<VideoCompetitor> videoCompetitors = db.VideoCompetitors.Include("Competition").ToList();
            int wonCompetitionNumber = 0;
            int lostCompetitionNumber = 0;
            for (var i = 0; i < videoCompetitors.Count(); i++)
            {
                if (videoCompetitors[i].MicrochipNumber == id)
                {
                    if (videoCompetitors[i].Winner == true)
                    {
                        wonCompetitionNumber = wonCompetitionNumber + 1; 
                    }
                    else
                    {
                        lostCompetitionNumber = lostCompetitionNumber + 1;
                    }
                }
              
            }
           

            List<PhotoCompetitor> photoCompetitors = db.PhotoCompetitors.Include("Competition").ToList();
           
            for (var i = 0; i < photoCompetitors.Count(); i++)
            {
                if (photoCompetitors[i].MicrochipNumber == id)
                {
                    if (photoCompetitors[i].Winner == false)
                    {
                        lostCompetitionNumber = lostCompetitionNumber + 1;
                    }
                    else
                    {
                        wonCompetitionNumber = wonCompetitionNumber + 1;
                    }
                }
            }
            chartData.Add(new object[] { "Competitii Pierdute", lostCompetitionNumber });
            chartData.Add(new object[] { "Competitii Catigate", wonCompetitionNumber });
            return Json(chartData, JsonRequestBehavior.AllowGet);


        }

        public ActionResult AboutOneCompetition(int id)
        {
            return View();
        }

        public JsonResult GetAge(int id)
        {
            List<object> chartData = new List<object>();
            chartData.Add(new object[] { "Varsta", "Numar concurenti cu aceasta varsta" });
            return Json(chartData, JsonRequestBehavior.AllowGet);
        }



    }
}