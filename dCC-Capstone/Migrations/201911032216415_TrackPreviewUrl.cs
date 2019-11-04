namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TrackPreviewUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tracks", "TrackPreviewUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tracks", "TrackPreviewUrl");
        }
    }
}
