using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Capstone.Models
{
    public class Genre
    {
        [Key]
        public int GenreId { get; set; }

        [Display(Name = "Genre Name")]
        public string GenreName { get; set; }

        [Display(Name = "Genre Spotify Name")]
        public string GenreSpotifyName { get; set; }

        [Display(Name = "Genre Listeners")]
        public List<Listener> GenreListeners { get; set; }

        [Display(Name = "Genre Artists")]
        public List<Artist> GenreArtists { get; set; }

        [Display(Name = "Genre Albums")]
        public List<Track> GenreAlbums { get; set; }
    }
}