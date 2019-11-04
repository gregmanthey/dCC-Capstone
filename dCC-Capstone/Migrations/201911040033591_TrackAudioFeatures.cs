namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TrackAudioFeatures : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tracks", "TrackAcousticness", c => c.Double());
            AddColumn("dbo.Tracks", "TrackInstrumentalness", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tracks", "TrackInstrumentalness");
            DropColumn("dbo.Tracks", "TrackAcousticness");
        }
    }
}
