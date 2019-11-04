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

        [Display(Name = "Number of tracks")]
        public int TrackCount
        {
            get
            {
                if (PlaylistTracks is null)
                {
                    return 0;
                }
                return PlaylistTracks.Count;
            }
        }
            

        [ForeignKey("Mood")]
        [Display(Name = "Playlist Mood")]
        public int? PlaylistMood { get; set; }
        public Mood Mood { get; set; }

        [Display(Name = "Genre Preference (100 being use my existing genres)")]
        [Range(0,100)]
        public int GenreWeightPercentage { get; set; }

        [Display(Name = "Popularity Preference (100 being most popular)")]
        [Range(0, 100)]
        public int PopularityTarget { get; set; }

        [Display(Name = "Allow only tracks with High Dynamic Range")]
        public bool DynamicTracksOnly { get; set; }

        [ForeignKey("Listener")]
        [Display(Name = "Created By")]
        public int CreatedBy { get; set; }
        public Listener Listener { get; set; }

        [Display(Name = "Playlist Tracks")]
        public List<Track> PlaylistTracks { get; set; }

        [Display(Name = "Playlist Genres")]
        public List<Genre> PlaylistGenres { get; set; }
    }
}