using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace dCC_Capstone.Models
{
    public class Playlist
    {
        [Key]
        public int PlaylistId { get; set; }
        public string PlaylistName { get; set; }
        public Mood PlaylistMood { get; set; }

        [ForeignKey("Listener")]
        public int CreatedBy { get; set; }
        public Listener Listener { get; set; }
        public virtual IList<Track> PlaylistTracks { get; set; }
        public virtual IList<Genre> PlaylistGenres { get; set; }
    }
}