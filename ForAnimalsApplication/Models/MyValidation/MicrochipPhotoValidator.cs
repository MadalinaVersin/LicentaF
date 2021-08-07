using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ForAnimalsApplication.Models.MyValidation
{
    public class MicrochipPhotoValidator : ValidationAttribute
    {
        private bool BeUniquePerCompetition(string microchip, int competitionId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var photoComp = db.PhotoCompetitors.ToList().Where(u => u.CompetitionId == competitionId && u.MicrochipNumber == microchip);

            if (photoComp.Count() == 0)
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
            PhotoCompetitor photoCompetitor = (PhotoCompetitor)validationContext.ObjectInstance;
            string microchip = photoCompetitor.MicrochipNumber;
            int compId = photoCompetitor.CompetitionId;
            
            if(BeUniquePerCompetition(microchip, compId) == false)
            {
                return new ValidationResult("Nu se poate sa concureze un animal de mai multe ori la aceeasi competitie!");
            }
            return ValidationResult.Success;
        }
    }
}