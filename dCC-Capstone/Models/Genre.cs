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
        public string GenreName { get; set; }
        public string GenreSpotifyName { get; set; }
        //public virtual IList<Listener> GenreListeners { get; set; }
        public virtual IList<Artist> GenreArtists { get; set; }
        //public virtual IList<Track> GenreTracks { get; set; }
        public int? ParentGenreId { get; set; }
        public virtual Genre ParentGenre { get; set; }
        public virtual IList<Genre> Children { get; set; }
    }
}