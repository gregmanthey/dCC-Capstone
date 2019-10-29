using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Capstone.Models
{
    public class ListenerTrack
    {
        [Key, Column(Order = 0)]
        public int ListenerId { get; set; }
        public virtual Listener Listener { get; set; }

        [Key, Column(Order = 1)]
        public int TrackId { get; set; }
        public virtual Track Track { get; set; }

        public bool TrackLiked { get; set; }
        public bool TrackDisliked { get; set; }
    }
}