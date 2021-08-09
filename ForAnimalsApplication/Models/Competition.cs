using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ForAnimalsApplication.Models.MyValidation;

namespace ForAnimalsApplication.Models
{
    public class Competition
    {
        [Key]
        public int CompetitionId { get; set; }
        [MinLength(2, ErrorMessage = "Numele competitiei nu poate sa aiba mai putin de 2 caractere!"),
         MaxLength(200, ErrorMessage = "Numele unei competitiei nu poate sa aiba mai mult de 200 de carcatere! ")]
        [Required(ErrorMessage = "Acest camp este obligatoriu!")]
        public string CompetitionName { get; set; }
        [Required(ErrorMessage ="Acest camp este obligatoriu!")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage ="Acest camp este obligatoriu!")]
        [EndDateValidator]
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public bool Evaluated { get; set; }

        //many-to-one relationship
        [Required(ErrorMessage ="Acest camp este obligatoriu!")]
        public int CompetitionTypeId { get; set; }
        public virtual CompetitionType CompetitionType { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem> CompetitionTypeList { get; set; }

        //Image
        //[Required(ErrorMessage = "Acest camp este obligatoriu!")]
        public string ImagePath { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }

        //one-to-many relationship
        public virtual ICollection<VideoCompetitor> VideoCompetitors { get; set; }

        //one-to-many relationship
        public virtual ICollection<PhotoCompetitor> PhotoCompetitors { get; set; }


    }
}