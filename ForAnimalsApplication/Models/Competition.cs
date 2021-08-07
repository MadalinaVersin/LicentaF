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
        public string CompetitionName { get; set; }
        public DateTime StartDate { get; set; }
        [EndDateValidator]
        public DateTime EndDate { get; set; }
        public string Description { get; set; }


        //many-to-one relationship
        public int CompetitionTypeId { get; set; }
        public virtual CompetitionType CompetitionType { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem> CompetitionTypeList { get; set; }

        //Image
        [DisplayName("Competition's image!")]
        public string ImagePath { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }

        //one-to-many relationship
        public virtual ICollection<VideoCompetitor> VideoCompetitors { get; set; }

        //one-to-many relationship
        public virtual ICollection<PhotoCompetitor> PhotoCompetitors { get; set; }


    }
}