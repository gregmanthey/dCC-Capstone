using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Capstone.Models
{
    public class Track
    {
        [Key]
        public int TrackId { get; set; }
        public string TrackSpotifyId { get; set; }
        public string TrackSpotifyUrl { get; set; }
        public string TrackName { get; set; }

        [ForeignKey("Artist")]
        [Display(Name = "Track Artist(s)")]
        public int? TrackArtistId { get; set; }
        public Artist Artist { get; set; }
        public IList<string> TrackArtistsSpotifyIds { get; set; }

        [Display(Name = "Album")]
        public int? TrackAlbumId { get; set; }
        public Album Album { get; set; }
        public string TrackAlbumSpotifyId { get; set; }
        public IList<Genre> TrackGenres { get; set; }

        [Display(Name = "Valence (Happiness)")]
        public double TrackValence { get; set; }

        [Display(Name = "Energy")]
        public double TrackEnergy { get; set; }

        [Display(Name = "Danceability")]
        public double TrackDanceability { get; set; }

        [Display(Name = "Loudness")]
        public double TrackLoudness { get; set; }

        [Display(Name = "Popularity")]
        public int TrackPopularity { get; set; }

        [Display(Name = "Tempo")]
        public double TrackTempo { get; set; }

        [Display(Name = "Duration (ms)")]
        public int TrackDurationInMs { get; set; }

        [Display(Name = "Mode: Is track in major key?")]
        public bool TrackIsInMajorKey { get; set; }
        public bool TrackChecked { get; set; }
    }
}