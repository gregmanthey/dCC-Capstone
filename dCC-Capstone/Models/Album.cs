using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dCC_Capstone.Models
{
    public class Album
    {
        [Key]
        public int AlbumId { get; set; }
        public IList<Track> AlbumTracks { get; set; }
        public Artist AlbumArtist { get; set; }
        public IList<Genre> AlbumGenres { get; set; }
    }
}