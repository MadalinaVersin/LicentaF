using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ForAnimalsApplication.Models.MyValidation;
namespace ForAnimalsApplication.Models
{
    public class PhotoCompetitor
    {
        [Key]
        public int PhotoCompetitorId { get; set; }

        [MicrochipPhotoValidator]
        public string MicrochipNumber { get; set; }
        [Required(ErrorMessage ="Acest camp este obligatoriu!")]
        public string Name { get; set; }

        public string Description { get; set; }

        public bool Winner { get; set; }

        [Required(ErrorMessage = "Acest camp este obligatoriu!")]
        public string Age { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem> AgeList { get; set; }

        [Required(ErrorMessage ="Acest camp este obligatoriu!")]
        public string Gender { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem> GenderList { get; set; }

        //Jury note
        public double JuryNote { get; set; }

        //FinalNote 
        public double FinalNote { get; set; }


        //Image
        public string ImagePath { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }

        //User
        //many-to-one relationship
        public string ApplicationUserID { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        //many-to-one relationship
        public int CompetitionId { get; set; }
        public virtual Competition Competition { get; set; }


        //one-to-many relationship
        public virtual ICollection<PhotoReview> PhotoReviews { get; set; }

    }
}