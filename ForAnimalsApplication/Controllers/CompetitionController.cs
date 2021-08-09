using ForAnimalsApplication.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ForAnimalsApplication.Controllers
{
    public class CompetitionController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Competition
        public ActionResult Index()
        {
            List<Competition> competitions = db.Competitions.Include("CompetitionType").ToList();
            ViewBag.Competitions = competitions;
            return View();
        }

        [HttpGet]
        public ActionResult New()
        {
            Competition competition = new Competition();
            competition.CompetitionTypeList = GetAllCompetitionType();
            return View(competition);
        }

        [HttpPost]
        public ActionResult New(Competition competitionReq)
        {

            try
            {
                competitionReq.CompetitionTypeList = GetAllCompetitionType();

                if (ModelState.IsValid)
                {
                    string fileName = Path.GetFileNameWithoutExtension(competitionReq.ImageFile.FileName);
                    string extension = Path.GetExtension(competitionReq.ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    competitionReq.ImagePath = "~/Images/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    competitionReq.ImageFile.SaveAs(fileName);

                    db.Competitions.Add(competitionReq);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(competitionReq);
            }
            catch (Exception e)
            {
                return View(competitionReq);
            }

        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                Competition competition = db.Competitions.Find(id);
                competition.CompetitionTypeList = GetAllCompetitionType();
                if (competition == null)
                {
                    return HttpNotFound("Coludn't find the competition with id " + id.ToString() + "!");
                }
                return View(competition);
            }
            return HttpNotFound("Missing competition id parameter!");
        }

        [HttpPut]
        public ActionResult Edit(int id, Competition competitionReq)
        {
            competitionReq.CompetitionTypeList = GetAllCompetitionType();

            // preluam competitia pe care vrem sa o modificam din baza de date
            Competition competition = db.Competitions.Include("CompetitionType")
                        .SingleOrDefault(b => b.CompetitionId.Equals(id));

            try
            {
                int updateImg = 0;
                if (competitionReq.ImageFile != null)
                {
                    updateImg = 1;
                    string fileName = Path.GetFileNameWithoutExtension(competitionReq.ImageFile.FileName);
                    string extension = Path.GetExtension(competitionReq.ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    competitionReq.ImagePath = "~/Images/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    competitionReq.ImageFile.SaveAs(fileName);

                }




                if (ModelState.IsValid)
                {
                    if (TryUpdateModel(competition))
                    {
                        competition.CompetitionName = competitionReq.CompetitionName;
                        competition.StartDate = competitionReq.StartDate;
                        competition.EndDate = competitionReq.EndDate;
                        competition.Description = competitionReq.Description;
                        competition.ImagePath = competitionReq.ImagePath;
                        if (updateImg == 1)
                        {
                            competition.ImageFile = competitionReq.ImageFile;
                            competition.ImagePath = competitionReq.ImagePath;
                        }
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                return View(competitionReq);
            }
            catch (Exception)
            {
                return View(competitionReq);
            }
        }

        public ActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                Competition competition = db.Competitions.Find(id);
                CompetitionType compType = db.CompetitionTypes.Find(competition.CompetitionTypeId);
                competition.CompetitionType = compType;
                if (competition != null)
                {
                    if (compType.Name == "Photo")
                    {
                        ViewBag.PhotoCompetitors = db.PhotoCompetitors.Include("ApplicationUser").Where(d => d.CompetitionId == id);
                    }
                    else
                    {
                        ViewBag.VideoCompetitors = db.VideoCompetitors.Include("ApplicationUser").Where(d => d.CompetitionId == id);
                    }
                    return View(competition);
                }
                return HttpNotFound("Couldn't find the animal with id " + id.ToString() + "!");
            }
            return HttpNotFound("Missing animal id parameter!");
        }

        [HttpDelete]
        public ActionResult Delete(int? id)
        {
            if (id.HasValue)
            {
                Competition competition = db.Competitions.Find(id);
                if (competition != null)
                {
                    db.Competitions.Remove(competition);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return HttpNotFound("Nu se poate gasi competitia cu id-ul:" + id.ToString());
            }
            return HttpNotFound("Id-ul competitiei lipseste!");
        }


        public IEnumerable<SelectListItem> GetAllCompetitionType()
        {
            var selectList = new List<SelectListItem>();
            foreach (var competitionType in db.CompetitionTypes.ToList())
            {
                selectList.Add(new SelectListItem
                {
                    Value = competitionType.CompetitionTypeId.ToString(),
                    Text = competitionType.Name
                });
            }
            return selectList;
        }

    }
}