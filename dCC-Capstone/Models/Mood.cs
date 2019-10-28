using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Capstone.Models
{
    public class Mood
    {
        [Key]
        public int MoodId { get; set; }

        public string MoodName { get; set; }

        public double MoodEnergyMinimum { get; set; }
        public double MoodEnergyMaximum { get; set; }
        public double MoodDanceabilityMinimum { get; set; }
        public double MoodDanceabilityMaximum { get; set; }

        public double MoodLoudnessMinimum { get; set; }
        public double MoodLoudnessMaximum { get; set; }
        public double MoodTempoMinimum { get; set; }
        public double MoodTempoMaximum { get; set; }
        public double ValenceMinimum { get; set; }
        public double ValenceMaximum { get; set; }
        public bool IsInMajorKeyMood { get; set; }
    }
}