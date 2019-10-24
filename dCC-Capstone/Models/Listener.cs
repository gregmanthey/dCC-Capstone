using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace dCC_Capstone.Models
{
    public class Listener
    {
        [Key]
        public int ListenerId { get; set; }
        [Index(IsUnique = true)]
        [MaxLength(30)]
        public string ScreenName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [ForeignKey("User")]
        [Index(IsUnique = true)]
        public string UserGuid { get; set; }
        public ApplicationUser User { get; set; }
        //public virtual IList<Artist> Artists { get; set; }
        //public virtual IList<Genre> Genres { get; set; }
        //public virtual IList<Track> Tracks { get; set; }
    }
}