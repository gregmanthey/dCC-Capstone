using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Capstone.Models
{
    public class Playlist
    {
        [Key]
        public int PlaylistId { get; set; }
        [Display(Name = "Private")]
        public bool IsPrivate { get; set; }
        [Display(Name = "Playlist Name")]
        public string PlaylistName { get; set; }
        [Display(Name = "Playlist Mood")]
        public Mood PlaylistMood { get; set; }
        [Display(Name = "Genre Preference")]
        [Range(0,100)]
        public int GenreWeightPercentage { get; set; }
        [Display(Name = "Popularity Preference")]
        [Range(0, 100)]
        public int PopularityWeightPercentage { get; set; }

        [Display(Name = "Allow only tracks with High Dynamic Range")]
        public bool DynamicTracksOnly { get; set; }

        [Display(Name = "")]
        [ForeignKey("Listener")]
        public int CreatedBy { get; set; }
        public Listener Listener { get; set; }
        public virtual IList<Track> PlaylistTracks { get; set; }
        public virtual IList<Genre> PlaylistGenres { get; set; }
    }
}