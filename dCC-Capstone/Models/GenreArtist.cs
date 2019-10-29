using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Capstone.Models
{
    public class GenreArtist
    {
        [Key, Column(Order = 0)]
        public int GenreId { get; set; }
        public virtual Genre Genre { get; set; }

        [Key, Column(Order = 1)]
        public int ArtistId { get; set; }
        public virtual Artist Artist { get; set; }
    }
}