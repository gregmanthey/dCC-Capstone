using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Capstone.Models
{
    public class Artist
    {
        [Key]
        public int ArtistId { get; set; }

        [Display(Name = "Artist Spotify ID")]
        public string ArtistSpotifyId { get; set; }

        [Display(Name = "Artist Spotify URL")]
        public string ArtistSpotifyUrl { get; set; }

        [Display(Name = "Artist Name")]
        public string ArtistName { get; set; }

        [Display(Name = "Artist Image URL")]
        public string ArtistImageUrl { get; set; }

        [Display(Name = "Artist Top Track Preview URL")]
        public string ArtistTopTrackPreviewUrl { get; set; }

        [Display(Name = "Artist Popularity")]
        public double ArtistPopularity { get; set; }
        public bool ArtistChecked { get; set; }

        [Display(Name = "Artist Genre")]
        public string SearchedGenre { get; set; }

        [Display(Name = "Artist Sub-genres")]
        public List<Genre> ArtistGenres { get; set; }

        [Display(Name = "Artist Listeners")]
        public List<Listener> ArtistListeners { get; set; }

        [Display(Name = "Artist Albums")]
        public List<Album> ArtistAlbums { get; set; }

    }
}