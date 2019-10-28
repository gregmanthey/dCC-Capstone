using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Capstone.Models
{
    public class ListenerGenre
    {
        [Key, Column(Order = 0)]
        public int ListenerID { get; set; }
        public virtual Listener Listener { get; set; }

        [Key, Column(Order = 1)]
        public int GenreID { get; set; }
        public virtual Genre Genre { get; set; }

        public bool GenreLiked { get; set; }
        public bool GenreDisliked { get; set; }
    }
}