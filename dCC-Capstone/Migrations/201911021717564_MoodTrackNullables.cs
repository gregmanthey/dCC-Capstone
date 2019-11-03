namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoodTrackNullables : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tracks", "TrackValence", c => c.Double());
            AlterColumn("dbo.Tracks", "TrackEnergy", c => c.Double());
            AlterColumn("dbo.Tracks", "TrackDanceability", c => c.Double());
            AlterColumn("dbo.Tracks", "TrackLoudness", c => c.Double());
            AlterColumn("dbo.Tracks", "TrackPopularity", c => c.Int());
            AlterColumn("dbo.Tracks", "TrackTempo", c => c.Double());
            AlterColumn("dbo.Tracks", "TrackDurationInMs", c => c.Int());
            AlterColumn("dbo.Moods", "MoodEnergyMinimum", c => c.Double());
            AlterColumn("dbo.Moods", "MoodEnergyMaximum", c => c.Double());
            AlterColumn("dbo.Moods", "MoodEnergyTarget", c => c.Double());
            AlterColumn("dbo.Moods", "MoodAcousticnessMinimum", c => c.Double());
            AlterColumn("dbo.Moods", "MoodAcousticnessMaximum", c => c.Double());
            AlterColumn("dbo.Moods", "MoodAcousticnessTarget", c => c.Double());
            AlterColumn("dbo.Moods", "MoodSpeechinessMinimum", c => c.Double());
            AlterColumn("dbo.Moods", "MoodSpeechinessMaximum", c => c.Double());
            AlterColumn("dbo.Moods", "MoodSpeechinessTarget", c => c.Double());
            AlterColumn("dbo.Moods", "MoodInstrumentalnessMinimum", c => c.Double());
            AlterColumn("dbo.Moods", "MoodInstrumentalnessMaximum", c => c.Double());
            AlterColumn("dbo.Moods", "MoodInstrumentalnessTarget", c => c.Double());
            AlterColumn("dbo.Moods", "MoodLivenessMinimum", c => c.Double());
            AlterColumn("dbo.Moods", "MoodLivenessMaximum", c => c.Double());
            AlterColumn("dbo.Moods", "MoodLivenessTarget", c => c.Double());
            AlterColumn("dbo.Moods", "MoodDanceabilityMinimum", c => c.Double());
            AlterColumn("dbo.Moods", "MoodDanceabilityMaximum", c => c.Double());
            AlterColumn("dbo.Moods", "MoodDanceabilityTarget", c => c.Double());
            AlterColumn("dbo.Moods", "MoodLoudnessMinimum", c => c.Double());
            AlterColumn("dbo.Moods", "MoodLoudnessMaximum", c => c.Double());
            AlterColumn("dbo.Moods", "MoodLoudnessTarget", c => c.Double());
            AlterColumn("dbo.Moods", "MoodTempoMinimum", c => c.Double());
            AlterColumn("dbo.Moods", "MoodTempoMaximum", c => c.Double());
            AlterColumn("dbo.Moods", "MoodTempoTarget", c => c.Double());
            AlterColumn("dbo.Moods", "MoodValenceMinimum", c => c.Double());
            AlterColumn("dbo.Moods", "MoodValenceMaximum", c => c.Double());
            AlterColumn("dbo.Moods", "MoodValenceTarget", c => c.Double());
            AlterColumn("dbo.Moods", "IsInMajorKeyMood", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Moods", "IsInMajorKeyMood", c => c.Int(nullable: false));
            AlterColumn("dbo.Moods", "MoodValenceTarget", c => c.Double(nullable: false));
            AlterColumn("dbo.Moods", "MoodValenceMaximum", c => c.Double(nullable: false));
            AlterColumn("dbo.Moods", "MoodValenceMinimum", c => c.Double(nullable: false));
            AlterColumn("dbo.Moods", "MoodTempoTarget", c => c.Double(nullable: false));
            AlterColumn("dbo.Moods", "MoodTempoMaximum", c => c.Double(nullable: false));
            AlterColumn("dbo.Moods", "MoodTempoMinimum", c => c.Double(nullable: false));
            AlterColumn("dbo.Moods", "MoodLoudnessTarget", c => c.Double(nullable: false));
            AlterColumn("dbo.Moods", "MoodLoudnessMaximum", c => c.Double(nullable: false));
            AlterColumn("dbo.Moods", "MoodLoudnessMinimum", c => c.Double(nullable: false));
            AlterColumn("dbo.Moods", "MoodDanceabilityTarget", c => c.Double(nullable: false));
            AlterColumn("dbo.Moods", "MoodDanceabilityMaximum", c => c.Double(nullable: false));
            AlterColumn("dbo.Moods", "MoodDanceabilityMinimum", c => c.Double(nullable: false));
            AlterColumn("dbo.Moods", "MoodLivenessTarget", c => c.Double(nullable: false));
            AlterColumn("dbo.Moods", "MoodLivenessMaximum", c => c.Double(nullable: false));
            AlterColumn("dbo.Moods", "MoodLivenessMinimum", c => c.Double(nullable: false));
            AlterColumn("dbo.Moods", "MoodInstrumentalnessTarget", c => c.Double(nullable: false));
            AlterColumn("dbo.Moods", "MoodInstrumentalnessMaximum", c => c.Double(nullable: false));
            AlterColumn("dbo.Moods", "MoodInstrumentalnessMinimum", c => c.Double(nullable: false));
            AlterColumn("dbo.Moods", "MoodSpeechinessTarget", c => c.Double(nullable: false));
            AlterColumn("dbo.Moods", "MoodSpeechinessMaximum", c => c.Double(nullable: false));
            AlterColumn("dbo.Moods", "MoodSpeechinessMinimum", c => c.Double(nullable: false));
            AlterColumn("dbo.Moods", "MoodAcousticnessTarget", c => c.Double(nullable: false));
            AlterColumn("dbo.Moods", "MoodAcousticnessMaximum", c => c.Double(nullable: false));
            AlterColumn("dbo.Moods", "MoodAcousticnessMinimum", c => c.Double(nullable: false));
            AlterColumn("dbo.Moods", "MoodEnergyTarget", c => c.Double(nullable: false));
            AlterColumn("dbo.Moods", "MoodEnergyMaximum", c => c.Double(nullable: false));
            AlterColumn("dbo.Moods", "MoodEnergyMinimum", c => c.Double(nullable: false));
            AlterColumn("dbo.Tracks", "TrackDurationInMs", c => c.Int(nullable: false));
            AlterColumn("dbo.Tracks", "TrackTempo", c => c.Double(nullable: false));
            AlterColumn("dbo.Tracks", "TrackPopularity", c => c.Int(nullable: false));
            AlterColumn("dbo.Tracks", "TrackLoudness", c => c.Double(nullable: false));
            AlterColumn("dbo.Tracks", "TrackDanceability", c => c.Double(nullable: false));
            AlterColumn("dbo.Tracks", "TrackEnergy", c => c.Double(nullable: false));
            AlterColumn("dbo.Tracks", "TrackValence", c => c.Double(nullable: false));
        }
    }
}
