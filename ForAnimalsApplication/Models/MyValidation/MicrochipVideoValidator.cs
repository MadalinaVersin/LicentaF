using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ForAnimalsApplication.Models.MyValidation
{
    public class MicrochipVideoValidator : ValidationAttribute
    {
        private bool BeUniquePerCompetition(string microchip, int competitionId, int competitorId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var videoComp = db.VideoCompetitors.ToList().Where(u => u.CompetitionId == competitionId && u.MicrochipNumber == microchip && u.VideoCompetitorId != competitorId);

            if (videoComp.Count() == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            VideoCompetitor videoCompetitor = (VideoCompetitor)validationContext.ObjectInstance;
            string microchip = videoCompetitor.MicrochipNumber;
            int compId = videoCompetitor.CompetitionId;
            int competitorId = videoCompetitor.VideoCompetitorId;

            if(microchip == null)
            {
                return new ValidationResult("Acest camp este obligatoriu!");
            }
            if (microchip.Length != 15)
            {
                return new ValidationResult("Numarul microcipului trebuie sa aiba 15 carcatere!");
            }
            string codRomania = microchip.Substring(0, 3);
            if (codRomania != "650")
            {
                return new ValidationResult("Codul tarei nu aparatine Romaniei!");
            }
            string zero = microchip.Substring(3, 1);
            if (zero != "0")
            {
                return new ValidationResult("Numarul microcipului nu este corect!");
            }
            if (BeUniquePerCompetition(microchip, compId, competitorId) == false)
            {
                return new ValidationResult("Nu se poate sa concureze un animal de mai multe ori la aceeasi competitie!");
            }
            return ValidationResult.Success;
        }
    }
}
