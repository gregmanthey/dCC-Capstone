namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoodAndTrackUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tracks", "TrackArtist_ArtistId", "dbo.Artists");
            DropIndex("dbo.Tracks", new[] { "TrackArtist_ArtistId" });
            RenameColumn(table: "dbo.Tracks", name: "TrackAlbum_AlbumId", newName: "Album_AlbumId");
            RenameIndex(table: "dbo.Tracks", name: "IX_TrackAlbum_AlbumId", newName: "IX_Album_AlbumId");
            AddColumn("dbo.Tracks", "TrackArtistSpotifyId", c => c.String());
            AddColumn("dbo.Tracks", "TrackAlbumSpotifyId", c => c.String());
            AddColumn("dbo.Tracks", "TrackPopularity", c => c.Int(nullable: false));
            AddColumn("dbo.Moods", "MoodEnergyTarget", c => c.Double(nullable: false));
            AddColumn("dbo.Moods", "MoodDanceabilityTarget", c => c.Double(nullable: false));
            AddColumn("dbo.Moods", "MoodLoudnessTarget", c => c.Double(nullable: false));
            AddColumn("dbo.Moods", "MoodTempoTarget", c => c.Double(nullable: false));
            AddColumn("dbo.Moods", "MoodValenceMinimum", c => c.Double(nullable: false));
            AddColumn("dbo.Moods", "MoodValenceMaximum", c => c.Double(nullable: false));
            AddColumn("dbo.Moods", "MoodValenceTarget", c => c.Double(nullable: false));
            AlterColumn("dbo.Moods", "IsInMajorKeyMood", c => c.Int(nullable: false));
            DropColumn("dbo.Tracks", "TrackArtist_ArtistId");
            DropColumn("dbo.Moods", "ValenceMinimum");
            DropColumn("dbo.Moods", "ValenceMaximum");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Moods", "ValenceMaximum", c => c.Double(nullable: false));
            AddColumn("dbo.Moods", "ValenceMinimum", c => c.Double(nullable: false));
            AddColumn("dbo.Tracks", "TrackArtist_ArtistId", c => c.Int());
            AlterColumn("dbo.Moods", "IsInMajorKeyMood", c => c.Boolean(nullable: false));
            DropColumn("dbo.Moods", "MoodValenceTarget");
            DropColumn("dbo.Moods", "MoodValenceMaximum");
            DropColumn("dbo.Moods", "MoodValenceMinimum");
            DropColumn("dbo.Moods", "MoodTempoTarget");
            DropColumn("dbo.Moods", "MoodLoudnessTarget");
            DropColumn("dbo.Moods", "MoodDanceabilityTarget");
            DropColumn("dbo.Moods", "MoodEnergyTarget");
            DropColumn("dbo.Tracks", "TrackPopularity");
            DropColumn("dbo.Tracks", "TrackAlbumSpotifyId");
            DropColumn("dbo.Tracks", "TrackArtistSpotifyId");
            RenameIndex(table: "dbo.Tracks", name: "IX_Album_AlbumId", newName: "IX_TrackAlbum_AlbumId");
            RenameColumn(table: "dbo.Tracks", name: "Album_AlbumId", newName: "TrackAlbum_AlbumId");
            CreateIndex("dbo.Tracks", "TrackArtist_ArtistId");
            AddForeignKey("dbo.Tracks", "TrackArtist_ArtistId", "dbo.Artists", "ArtistId");
        }
    }
}
