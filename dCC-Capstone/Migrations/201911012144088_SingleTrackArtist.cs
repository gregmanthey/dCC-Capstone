namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SingleTrackArtist : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Artists", "Track_TrackId", "dbo.Tracks");
            DropIndex("dbo.Artists", new[] { "Track_TrackId" });
            AddColumn("dbo.Tracks", "TrackArtistId", c => c.Int());
            AlterColumn("dbo.Tracks", "TrackAlbumId", c => c.Int());
            CreateIndex("dbo.Tracks", "TrackArtistId");
            AddForeignKey("dbo.Tracks", "TrackArtistId", "dbo.Artists", "ArtistId");
            DropColumn("dbo.Artists", "Track_TrackId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Artists", "Track_TrackId", c => c.Int());
            DropForeignKey("dbo.Tracks", "TrackArtistId", "dbo.Artists");
            DropIndex("dbo.Tracks", new[] { "TrackArtistId" });
            AlterColumn("dbo.Tracks", "TrackAlbumId", c => c.Int(nullable: false));
            DropColumn("dbo.Tracks", "TrackArtistId");
            CreateIndex("dbo.Artists", "Track_TrackId");
            AddForeignKey("dbo.Artists", "Track_TrackId", "dbo.Tracks", "TrackId");
        }
    }
}
