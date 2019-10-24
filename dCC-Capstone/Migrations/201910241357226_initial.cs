namespace dCC_Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Albums",
                c => new
                    {
                        AlbumId = c.Int(nullable: false, identity: true),
                        AlbumSpotifyId = c.String(),
                        AlbumArtist_ArtistId = c.Int(),
                    })
                .PrimaryKey(t => t.AlbumId)
                .ForeignKey("dbo.Artists", t => t.AlbumArtist_ArtistId)
                .Index(t => t.AlbumArtist_ArtistId);
            
            CreateTable(
                "dbo.Artists",
                c => new
                    {
                        ArtistId = c.Int(nullable: false, identity: true),
                        ArtistSpotifyId = c.String(),
                        ArtistName = c.String(),
                    })
                .PrimaryKey(t => t.ArtistId);
            
            CreateTable(
                "dbo.Tracks",
                c => new
                    {
                        TrackId = c.Int(nullable: false, identity: true),
                        TrackSpotifyId = c.String(),
                        TrackName = c.String(),
                        TrackValence = c.Double(nullable: false),
                        TrackEnergy = c.Double(nullable: false),
                        TrackDanceability = c.Double(nullable: false),
                        TrackLoudness = c.Double(nullable: false),
                        TrackTempo = c.Double(nullable: false),
                        TrackDurationInMs = c.Int(nullable: false),
                        TrackIsInMajorKey = c.Boolean(nullable: false),
                        TrackLiked = c.Boolean(nullable: false),
                        TrackDisliked = c.Boolean(nullable: false),
                        TrackAlbum_AlbumId = c.Int(),
                        TrackArtist_ArtistId = c.Int(),
                        Playlist_PlaylistId = c.Int(),
                    })
                .PrimaryKey(t => t.TrackId)
                .ForeignKey("dbo.Albums", t => t.TrackAlbum_AlbumId)
                .ForeignKey("dbo.Artists", t => t.TrackArtist_ArtistId)
                .ForeignKey("dbo.Playlists", t => t.Playlist_PlaylistId)
                .Index(t => t.TrackAlbum_AlbumId)
                .Index(t => t.TrackArtist_ArtistId)
                .Index(t => t.Playlist_PlaylistId);
            
            CreateTable(
                "dbo.GenreArtists",
                c => new
                    {
                        GenreID = c.Int(nullable: false),
                        ArtistID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.GenreID, t.ArtistID })
                .ForeignKey("dbo.Artists", t => t.ArtistID, cascadeDelete: true)
                .ForeignKey("dbo.Genres", t => t.GenreID, cascadeDelete: true)
                .Index(t => t.GenreID)
                .Index(t => t.ArtistID);
            
            CreateTable(
                "dbo.Genres",
                c => new
                    {
                        GenreId = c.Int(nullable: false, identity: true),
                        GenreName = c.String(),
                        GenreSpotifyName = c.String(),
                        ParentGenreId = c.Int(),
                        Playlist_PlaylistId = c.Int(),
                    })
                .PrimaryKey(t => t.GenreId)
                .ForeignKey("dbo.Genres", t => t.ParentGenreId)
                .ForeignKey("dbo.Playlists", t => t.Playlist_PlaylistId)
                .Index(t => t.ParentGenreId)
                .Index(t => t.Playlist_PlaylistId);
            
            CreateTable(
                "dbo.GenreTracks",
                c => new
                    {
                        GenreID = c.Int(nullable: false),
                        TrackID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.GenreID, t.TrackID })
                .ForeignKey("dbo.Genres", t => t.GenreID, cascadeDelete: true)
                .ForeignKey("dbo.Tracks", t => t.TrackID, cascadeDelete: true)
                .Index(t => t.GenreID)
                .Index(t => t.TrackID);
            
            CreateTable(
                "dbo.ListenerArtists",
                c => new
                    {
                        ListenerID = c.Int(nullable: false),
                        ArtistID = c.Int(nullable: false),
                        ArtistLiked = c.Boolean(nullable: false),
                        ArtistDisliked = c.Boolean(nullable: false),
                        FavoriteArtist = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.ListenerID, t.ArtistID })
                .ForeignKey("dbo.Artists", t => t.ArtistID, cascadeDelete: true)
                .ForeignKey("dbo.Listeners", t => t.ListenerID, cascadeDelete: true)
                .Index(t => t.ListenerID)
                .Index(t => t.ArtistID);
            
            CreateTable(
                "dbo.Listeners",
                c => new
                    {
                        ListenerId = c.Int(nullable: false, identity: true),
                        ScreenName = c.String(maxLength: 30),
                        FirstName = c.String(),
                        LastName = c.String(),
                        UserGuid = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ListenerId)
                .ForeignKey("dbo.ApplicationUsers", t => t.UserGuid)
                .Index(t => t.ScreenName, unique: true)
                .Index(t => t.UserGuid, unique: true);
            
            CreateTable(
                "dbo.ApplicationUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentityUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        IdentityRole_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.IdentityRoles", t => t.IdentityRole_Id)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.IdentityRole_Id);
            
            CreateTable(
                "dbo.ListenerGenres",
                c => new
                    {
                        ListenerID = c.Int(nullable: false),
                        GenreID = c.Int(nullable: false),
                        GenreLiked = c.Boolean(nullable: false),
                        GenreDisliked = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.ListenerID, t.GenreID })
                .ForeignKey("dbo.Genres", t => t.GenreID, cascadeDelete: true)
                .ForeignKey("dbo.Listeners", t => t.ListenerID, cascadeDelete: true)
                .Index(t => t.ListenerID)
                .Index(t => t.GenreID);
            
            CreateTable(
                "dbo.ListenerTracks",
                c => new
                    {
                        ListenerID = c.Int(nullable: false),
                        TrackID = c.Int(nullable: false),
                        TrackLiked = c.Boolean(nullable: false),
                        TrackDisliked = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.ListenerID, t.TrackID })
                .ForeignKey("dbo.Listeners", t => t.ListenerID, cascadeDelete: true)
                .ForeignKey("dbo.Tracks", t => t.TrackID, cascadeDelete: true)
                .Index(t => t.ListenerID)
                .Index(t => t.TrackID);
            
            CreateTable(
                "dbo.Moods",
                c => new
                    {
                        MoodId = c.Int(nullable: false, identity: true),
                        MoodName = c.String(),
                        MoodEnergyMinimum = c.Double(nullable: false),
                        MoodEnergyMaximum = c.Double(nullable: false),
                        MoodDanceabilityMinimum = c.Double(nullable: false),
                        MoodDanceabilityMaximum = c.Double(nullable: false),
                        MoodLoudnessMinimum = c.Double(nullable: false),
                        MoodLoudnessMaximum = c.Double(nullable: false),
                        MoodTempoMinimum = c.Double(nullable: false),
                        MoodTempoMaximum = c.Double(nullable: false),
                        ValenceMinimum = c.Double(nullable: false),
                        ValenceMaximum = c.Double(nullable: false),
                        IsInMajorKeyMood = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.MoodId);
            
            CreateTable(
                "dbo.Playlists",
                c => new
                    {
                        PlaylistId = c.Int(nullable: false, identity: true),
                        PlaylistName = c.String(),
                        CreatedBy = c.Int(nullable: false),
                        PlaylistMood_MoodId = c.Int(),
                    })
                .PrimaryKey(t => t.PlaylistId)
                .ForeignKey("dbo.Listeners", t => t.CreatedBy, cascadeDelete: true)
                .ForeignKey("dbo.Moods", t => t.PlaylistMood_MoodId)
                .Index(t => t.CreatedBy)
                .Index(t => t.PlaylistMood_MoodId);
            
            CreateTable(
                "dbo.IdentityRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "IdentityRole_Id", "dbo.IdentityRoles");
            DropForeignKey("dbo.Tracks", "Playlist_PlaylistId", "dbo.Playlists");
            DropForeignKey("dbo.Playlists", "PlaylistMood_MoodId", "dbo.Moods");
            DropForeignKey("dbo.Genres", "Playlist_PlaylistId", "dbo.Playlists");
            DropForeignKey("dbo.Playlists", "CreatedBy", "dbo.Listeners");
            DropForeignKey("dbo.ListenerTracks", "TrackID", "dbo.Tracks");
            DropForeignKey("dbo.ListenerTracks", "ListenerID", "dbo.Listeners");
            DropForeignKey("dbo.ListenerGenres", "ListenerID", "dbo.Listeners");
            DropForeignKey("dbo.ListenerGenres", "GenreID", "dbo.Genres");
            DropForeignKey("dbo.ListenerArtists", "ListenerID", "dbo.Listeners");
            DropForeignKey("dbo.Listeners", "UserGuid", "dbo.ApplicationUsers");
            DropForeignKey("dbo.AspNetUserRoles", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.AspNetUserLogins", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.IdentityUserClaims", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ListenerArtists", "ArtistID", "dbo.Artists");
            DropForeignKey("dbo.GenreTracks", "TrackID", "dbo.Tracks");
            DropForeignKey("dbo.GenreTracks", "GenreID", "dbo.Genres");
            DropForeignKey("dbo.GenreArtists", "GenreID", "dbo.Genres");
            DropForeignKey("dbo.Genres", "ParentGenreId", "dbo.Genres");
            DropForeignKey("dbo.GenreArtists", "ArtistID", "dbo.Artists");
            DropForeignKey("dbo.Tracks", "TrackArtist_ArtistId", "dbo.Artists");
            DropForeignKey("dbo.Tracks", "TrackAlbum_AlbumId", "dbo.Albums");
            DropForeignKey("dbo.Albums", "AlbumArtist_ArtistId", "dbo.Artists");
            DropIndex("dbo.Playlists", new[] { "PlaylistMood_MoodId" });
            DropIndex("dbo.Playlists", new[] { "CreatedBy" });
            DropIndex("dbo.ListenerTracks", new[] { "TrackID" });
            DropIndex("dbo.ListenerTracks", new[] { "ListenerID" });
            DropIndex("dbo.ListenerGenres", new[] { "GenreID" });
            DropIndex("dbo.ListenerGenres", new[] { "ListenerID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "IdentityRole_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserClaims", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Listeners", new[] { "UserGuid" });
            DropIndex("dbo.Listeners", new[] { "ScreenName" });
            DropIndex("dbo.ListenerArtists", new[] { "ArtistID" });
            DropIndex("dbo.ListenerArtists", new[] { "ListenerID" });
            DropIndex("dbo.GenreTracks", new[] { "TrackID" });
            DropIndex("dbo.GenreTracks", new[] { "GenreID" });
            DropIndex("dbo.Genres", new[] { "Playlist_PlaylistId" });
            DropIndex("dbo.Genres", new[] { "ParentGenreId" });
            DropIndex("dbo.GenreArtists", new[] { "ArtistID" });
            DropIndex("dbo.GenreArtists", new[] { "GenreID" });
            DropIndex("dbo.Tracks", new[] { "Playlist_PlaylistId" });
            DropIndex("dbo.Tracks", new[] { "TrackArtist_ArtistId" });
            DropIndex("dbo.Tracks", new[] { "TrackAlbum_AlbumId" });
            DropIndex("dbo.Albums", new[] { "AlbumArtist_ArtistId" });
            DropTable("dbo.IdentityRoles");
            DropTable("dbo.Playlists");
            DropTable("dbo.Moods");
            DropTable("dbo.ListenerTracks");
            DropTable("dbo.ListenerGenres");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.IdentityUserClaims");
            DropTable("dbo.ApplicationUsers");
            DropTable("dbo.Listeners");
            DropTable("dbo.ListenerArtists");
            DropTable("dbo.GenreTracks");
            DropTable("dbo.Genres");
            DropTable("dbo.GenreArtists");
            DropTable("dbo.Tracks");
            DropTable("dbo.Artists");
            DropTable("dbo.Albums");
        }
    }
}
