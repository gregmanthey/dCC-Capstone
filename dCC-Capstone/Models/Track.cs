﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Capstone.Models
{
    public class Track
    {
        [Key]
        public int TrackId { get; set; }
        public string TrackSpotifyId { get; set; }
        public string TrackName { get; set; }
        public Artist TrackArtist { get; set; }
        public Album TrackAlbum { get; set; }
        //public virtual IList<Genre> TrackGenres { get; set; }
        public double TrackValence { get; set; }
        public double TrackEnergy { get; set; }
        public double TrackDanceability { get; set; }
        public double TrackLoudness { get; set; }
        public double TrackTempo { get; set; }
        public int TrackDurationInMs { get; set; }
        public bool TrackIsInMajorKey { get; set; }
        public bool TrackLiked { get; set; }
        public bool TrackDisliked { get; set; }
    }
}