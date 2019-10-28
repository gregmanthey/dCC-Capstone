using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Capstone.Models
{
    public class ListenerArtist
    {
        [Key, Column(Order = 0)]
        public int ListenerID { get; set; }
        public virtual Listener Listener { get; set; }

        [Key, Column(Order = 1)]
        public int ArtistID { get; set; }
        public virtual Artist Artist { get; set; }

        public bool ArtistLiked { get; set; }
        public bool ArtistDisliked { get; set; }
        public bool FavoriteArtist { get; set; }
    }
}