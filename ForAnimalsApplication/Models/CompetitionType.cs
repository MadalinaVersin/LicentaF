using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ForAnimalsApplication.Models
{
    public class CompetitionType
    {
        [Key]
        public int CompetitionTypeId { get; set; }
        public string Name { get; set; }


        //one-to-many
        public virtual ICollection<Competition> Competitions { get; set; }
    }
}