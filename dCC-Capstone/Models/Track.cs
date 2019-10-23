using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dCC_Capstone.Models
{
    public class Track
    {
        [Key]
        public int TrackId { get; set; }
        public string TrackName { get; set; }
        public Artist TrackArtist { get; set; }
        public IList<Genre> TrackGenres { get; set; }
        public double TrackEnergy { get; set; }
        public double TrackDanceability { get; set; }
        public double TrackLoudness { get; set; }
        public double TrackTempo { get; set; }
        public int TrackDurationInMs { get; set; }
        public bool IsInMajorKey { get; set; }
        public string TrackSpotifyId { get; set; }
    }
}