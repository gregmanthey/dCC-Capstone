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

        [Display(Name = "Album Title")]
        public string AlbumName { get; set; }
        public string AlbumSpotifyId { get; set; }

        [Display(Name = "Album Spotify URL")]
        public string AlbumSpotifyUrl { get; set; }

        [Display(Name = "Album Image URL")]
        public string AlbumImageUrl { get; set; }

        [Display(Name = "Album Tracks")]
        public IList<Track> AlbumTracks { get; set; }

        [Display(Name = "Album Genres")]
        public IList<Genre> AlbumGenres { get; set; }

        [Display(Name = "Album Artists")]
        public IList<Artist> AlbumArtists { get; set; }
    }
}