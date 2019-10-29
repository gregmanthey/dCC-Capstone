namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPlaylistTracks : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.GenreArtists", new[] { "GenreID" });
            DropIndex("dbo.GenreArtists", new[] { "ArtistID" });
            DropIndex("dbo.GenreTracks", new[] { "GenreID" });
            DropIndex("dbo.GenreTracks", new[] { "TrackID" });
            DropIndex("dbo.ListenerArtists", new[] { "ListenerID" });
            DropIndex("dbo.ListenerArtists", new[] { "ArtistID" });
            DropIndex("dbo.ListenerGenres", new[] { "ListenerID" });
            DropIndex("dbo.ListenerGenres", new[] { "GenreID" });
            DropIndex("dbo.ListenerTracks", new[] { "ListenerID" });
            DropIndex("dbo.ListenerTracks", new[] { "TrackID" });
            CreateTable(
                "dbo.PlaylistTracks",
                c => new
                    {
                        ListenerId = c.Int(nullable: false),
                        TrackId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ListenerId, t.TrackId })
                .ForeignKey("dbo.Listeners", t => t.ListenerId, cascadeDelete: true)
                .ForeignKey("dbo.Tracks", t => t.TrackId, cascadeDelete: true)
                .Index(t => t.ListenerId)
                .Index(t => t.TrackId);
            
            AddColumn("dbo.Playlists", "GenreWeightPercentage", c => c.Int(nullable: false));
            AddColumn("dbo.Playlists", "PopularityWeightPercentage", c => c.Int(nullable: false));
            CreateIndex("dbo.GenreArtists", "GenreId");
            CreateIndex("dbo.GenreArtists", "ArtistId");
            CreateIndex("dbo.GenreTracks", "GenreId");
            CreateIndex("dbo.GenreTracks", "TrackId");
            CreateIndex("dbo.ListenerArtists", "ListenerId");
            CreateIndex("dbo.ListenerArtists", "ArtistId");
            CreateIndex("dbo.ListenerGenres", "ListenerId");
            CreateIndex("dbo.ListenerGenres", "GenreId");
            CreateIndex("dbo.ListenerTracks", "ListenerId");
            CreateIndex("dbo.ListenerTracks", "TrackId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PlaylistTracks", "TrackId", "dbo.Tracks");
            DropForeignKey("dbo.PlaylistTracks", "ListenerId", "dbo.Listeners");
            DropIndex("dbo.PlaylistTracks", new[] { "TrackId" });
            DropIndex("dbo.PlaylistTracks", new[] { "ListenerId" });
            DropIndex("dbo.ListenerTracks", new[] { "TrackId" });
            DropIndex("dbo.ListenerTracks", new[] { "ListenerId" });
            DropIndex("dbo.ListenerGenres", new[] { "GenreId" });
            DropIndex("dbo.ListenerGenres", new[] { "ListenerId" });
            DropIndex("dbo.ListenerArtists", new[] { "ArtistId" });
            DropIndex("dbo.ListenerArtists", new[] { "ListenerId" });
            DropIndex("dbo.GenreTracks", new[] { "TrackId" });
            DropIndex("dbo.GenreTracks", new[] { "GenreId" });
            DropIndex("dbo.GenreArtists", new[] { "ArtistId" });
            DropIndex("dbo.GenreArtists", new[] { "GenreId" });
            DropColumn("dbo.Playlists", "PopularityWeightPercentage");
            DropColumn("dbo.Playlists", "GenreWeightPercentage");
            DropTable("dbo.PlaylistTracks");
            CreateIndex("dbo.ListenerTracks", "TrackID");
            CreateIndex("dbo.ListenerTracks", "ListenerID");
            CreateIndex("dbo.ListenerGenres", "GenreID");
            CreateIndex("dbo.ListenerGenres", "ListenerID");
            CreateIndex("dbo.ListenerArtists", "ArtistID");
            CreateIndex("dbo.ListenerArtists", "ListenerID");
            CreateIndex("dbo.GenreTracks", "TrackID");
            CreateIndex("dbo.GenreTracks", "GenreID");
            CreateIndex("dbo.GenreArtists", "ArtistID");
            CreateIndex("dbo.GenreArtists", "GenreID");
        }
    }
}
