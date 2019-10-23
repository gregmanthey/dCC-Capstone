using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dCC_Capstone.Models
{
    public class MusicEnthusiast
    {
        [Key]
        public int MusicEnthusiastId { get; set; }
        public string ScreenName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IList<Artist> FavoriteArtists { get; set; }
        public IList<Artist> LikedArtists { get; set; }
        public IList<Genre> FavoriteGenres { get; set; }
        public IList<Genre> LikedGenres { get; set; }
        public IList<Track> LikedTracks { get; set; }
    }
}