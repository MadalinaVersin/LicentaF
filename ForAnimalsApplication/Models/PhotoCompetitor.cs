using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ForAnimalsApplication.Models
{
    public class PhotoCompetitor
    {
        [Key]
        public int PhotoCompetitorId { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        //Jury note
        public int JuryNote { get; set; }

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