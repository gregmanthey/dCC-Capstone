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
        public double MoodEnergyTarget { get; set; }
        public double MoodAcousticnessMinimum { get; set; }
        public double MoodAcousticnessMaximum { get; set; }
        public double MoodAcousticnessTarget { get; set; }
        public double MoodSpeechinessMinimum { get; set; }
        public double MoodSpeechinessMaximum { get; set; }
        public double MoodSpeechinessTarget { get; set; }
        public double MoodInstrumentalnessMinimum { get; set; }
        public double MoodInstrumentalnessMaximum { get; set; }
        public double MoodInstrumentalnessTarget { get; set; }
        public double MoodLivenessMinimum { get; set; }
        public double MoodLivenessMaximum { get; set; }
        public double MoodLivenessTarget { get; set; }
        public double MoodDanceabilityMinimum { get; set; }
        public double MoodDanceabilityMaximum { get; set; }
        public double MoodDanceabilityTarget { get; set; }

        public double MoodLoudnessMinimum { get; set; }
        public double MoodLoudnessMaximum { get; set; }
        public double MoodLoudnessTarget { get; set; }
        public double MoodTempoMinimum { get; set; }
        public double MoodTempoMaximum { get; set; }
        public double MoodTempoTarget { get; set; }
        public double MoodValenceMinimum { get; set; }
        public double MoodValenceMaximum { get; set; }
        public double MoodValenceTarget { get; set; }
        public int IsInMajorKeyMood { get; set; }
    }
}