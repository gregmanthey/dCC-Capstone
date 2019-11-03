namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ManyToManyTrackPlaylist : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tracks", "Playlist_PlaylistId", "dbo.Playlists");
            DropIndex("dbo.Tracks", new[] { "Playlist_PlaylistId" });
            CreateTable(
                "dbo.PlaylistTracks",
                c => new
                    {
                        Playlist_PlaylistId = c.Int(nullable: false),
                        Track_TrackId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Playlist_PlaylistId, t.Track_TrackId })
                .ForeignKey("dbo.Playlists", t => t.Playlist_PlaylistId, cascadeDelete: true)
                .ForeignKey("dbo.Tracks", t => t.Track_TrackId, cascadeDelete: true)
                .Index(t => t.Playlist_PlaylistId)
                .Index(t => t.Track_TrackId);
            
            DropColumn("dbo.Tracks", "Playlist_PlaylistId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tracks", "Playlist_PlaylistId", c => c.Int());
            DropForeignKey("dbo.PlaylistTracks", "Track_TrackId", "dbo.Tracks");
            DropForeignKey("dbo.PlaylistTracks", "Playlist_PlaylistId", "dbo.Playlists");
            DropIndex("dbo.PlaylistTracks", new[] { "Track_TrackId" });
            DropIndex("dbo.PlaylistTracks", new[] { "Playlist_PlaylistId" });
            DropTable("dbo.PlaylistTracks");
            CreateIndex("dbo.Tracks", "Playlist_PlaylistId");
            AddForeignKey("dbo.Tracks", "Playlist_PlaylistId", "dbo.Playlists", "PlaylistId");
        }
    }
}
