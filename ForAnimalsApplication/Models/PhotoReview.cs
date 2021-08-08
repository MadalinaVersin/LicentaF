using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ForAnimalsApplication.Models
{
    public class PhotoReview
    {
            public PhotoReview()
            {
                ReviewDate = DateTime.Now;
            }
            [Key]
            public int PhotoReviewId { get; set; }

            [Required(ErrorMessage ="Trebuie sa lasati si un comentariu!")]
            public string Text { get; set; }

            public DateTime ReviewDate { get; set; }
            
            [Required(ErrorMessage ="Acest camp este obligatoriu!")]
            public int Note { get; set; }

            [NotMapped]
            public IEnumerable<SelectListItem> NoteList { get; set; }

            //many-to-one relationship
            public string ApplicationUserID { get; set; }
            public virtual ApplicationUser ApplicationUser { get; set; }

            //many-to-one relationship
            public int PhotoCompetitorId { get; set; }
            public virtual PhotoCompetitor PhotoCompetitor { get; set; }
        }
    }
