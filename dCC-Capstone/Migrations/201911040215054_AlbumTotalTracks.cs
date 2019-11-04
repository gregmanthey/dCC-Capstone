namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlbumTotalTracks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Albums", "AlbumTotalTracks", c => c.Int(nullable: false));
            AddColumn("dbo.Tracks", "TrackDiscNumber", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tracks", "TrackDiscNumber");
            DropColumn("dbo.Albums", "AlbumTotalTracks");
        }
    }
}
