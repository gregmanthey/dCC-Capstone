using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Capstone.Models
{
    public class Album
    {
        [Key]
        public int AlbumId { get; set; }
        public string AlbumSpotifyId { get; set; }
        public string AlbumImageUrl { get; set; }
        public virtual IList<Track> AlbumTracks { get; set; }
        public Artist AlbumArtist { get; set; }
    }
}