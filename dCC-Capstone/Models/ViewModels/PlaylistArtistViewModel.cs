using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Models.ViewModels
{
    public class PlaylistArtistViewModel
    {
        public Playlist Playlist { get; set; }
        public List<Album> Albums { get; set; }
        public List<Artist> Artists { get; set; }
    }
}