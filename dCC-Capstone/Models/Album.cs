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
        public List<Track> AlbumTracks { get; set; }

        [Display(Name = "Album Genres")]
        public List<Genre> AlbumGenres { get; set; }

        [Display(Name = "Album Artists")]
        public List<Artist> AlbumArtists { get; set; }
    }
}