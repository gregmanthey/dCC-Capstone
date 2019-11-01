using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Capstone.Models
{
    public class Artist
    {
        [Key]
        public int ArtistId { get; set; }
        public string ArtistSpotifyId { get; set; }
        public string ArtistSpotifyUrl { get; set; }
        public string ArtistName { get; set; }
        public string ArtistImageUrl { get; set; }
        public string ArtistTopTrackPreviewUrl { get; set; }
        public double Popularity { get; set; }
        public bool Checked { get; set; }
        public virtual IList<Genre> ArtistGenres { get; set; }
        //public virtual IList<Listener> Fans { get; set; }

    }
}