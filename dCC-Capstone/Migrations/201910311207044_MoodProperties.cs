namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoodProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Moods", "MoodAcousticnessMinimum", c => c.Double(nullable: false));
            AddColumn("dbo.Moods", "MoodAcousticnessMaximum", c => c.Double(nullable: false));
            AddColumn("dbo.Moods", "MoodAcousticnessTarget", c => c.Double(nullable: false));
            AddColumn("dbo.Moods", "MoodSpeechinessMinimum", c => c.Double(nullable: false));
            AddColumn("dbo.Moods", "MoodSpeechinessMaximum", c => c.Double(nullable: false));
            AddColumn("dbo.Moods", "MoodSpeechinessTarget", c => c.Double(nullable: false));
            AddColumn("dbo.Moods", "MoodInstrumentalnessMinimum", c => c.Double(nullable: false));
            AddColumn("dbo.Moods", "MoodInstrumentalnessMaximum", c => c.Double(nullable: false));
            AddColumn("dbo.Moods", "MoodInstrumentalnessTarget", c => c.Double(nullable: false));
            AddColumn("dbo.Moods", "MoodLivenessMinimum", c => c.Double(nullable: false));
            AddColumn("dbo.Moods", "MoodLivenessMaximum", c => c.Double(nullable: false));
            AddColumn("dbo.Moods", "MoodLivenessTarget", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Moods", "MoodLivenessTarget");
            DropColumn("dbo.Moods", "MoodLivenessMaximum");
            DropColumn("dbo.Moods", "MoodLivenessMinimum");
            DropColumn("dbo.Moods", "MoodInstrumentalnessTarget");
            DropColumn("dbo.Moods", "MoodInstrumentalnessMaximum");
            DropColumn("dbo.Moods", "MoodInstrumentalnessMinimum");
            DropColumn("dbo.Moods", "MoodSpeechinessTarget");
            DropColumn("dbo.Moods", "MoodSpeechinessMaximum");
            DropColumn("dbo.Moods", "MoodSpeechinessMinimum");
            DropColumn("dbo.Moods", "MoodAcousticnessTarget");
            DropColumn("dbo.Moods", "MoodAcousticnessMaximum");
            DropColumn("dbo.Moods", "MoodAcousticnessMinimum");
        }
    }
}
