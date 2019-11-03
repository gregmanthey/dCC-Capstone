using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Capstone.Models
{
    public class Listener
    {
        [Key]
        public int ListenerId { get; set; }

        [Display(Name = "Screen Name")]
        [Index(IsUnique = true)]
        [MaxLength(30)]
        public string ScreenName { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        [ForeignKey("User")]
        [Index(IsUnique = true)]
        public string UserGuid { get; set; }
        public ApplicationUser User { get; set; }

        [Display(Name = "Listener Artists")]
        public List<Artist> ListenerArtists { get; set; }

        [Display(Name = "Listener Genres")]
        public List<Genre> ListenerGenres { get; set; }

        [Display(Name = "Listener Tracks")]
        public List<Track> ListenerTracks { get; set; }
    }
}