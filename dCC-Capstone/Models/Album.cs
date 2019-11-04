using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Display(Name ="Total Tracks")]
        public int AlbumTotalTracks { get; set; }

        [Display(Name = "Album Tracks")]
        public List<Track> AlbumTracks { get; set; }

        [Display(Name = "Album Genres")]
        public List<Genre> AlbumGenres { get; set; }

        [ForeignKey("Artist")]
        [Display(Name = "Album Artist")]
        public int AlbumArtistId { get; set; }
        public Artist Artist { get; set; }
    }
}