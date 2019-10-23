using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dCC_Capstone.Models
{
    public class Playlist
    {
        [Key]
        public int PlaylistId { get; set; }
        public string PlaylistName { get; set; }
        public IList<Track> PlaylistTracks { get; set; }
        public IList<Genre> PlaylistGenres { get; set; }
    }
}