namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class refactored : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Albums",
                c => new
                    {
                        AlbumId = c.Int(nullable: false, identity: true),
                        AlbumName = c.String(),
                        AlbumSpotifyId = c.String(),
                        AlbumSpotifyUrl = c.String(),
                        AlbumImageUrl = c.String(),
                    })
                .PrimaryKey(t => t.AlbumId);
            
            CreateTable(
                "dbo.Artists",
                c => new
                    {
                        ArtistId = c.Int(nullable: false, identity: true),
                        ArtistSpotifyId = c.String(),
                        ArtistSpotifyUrl = c.String(),
                        ArtistName = c.String(),
                        ArtistImageUrl = c.String(),
                        ArtistTopTrackPreviewUrl = c.String(),
                        ArtistPopularity = c.Double(nullable: false),
                        ArtistChecked = c.Boolean(nullable: false),
                        SearchedGenre = c.String(),
                        Track_TrackId = c.Int(),
                    })
                .PrimaryKey(t => t.ArtistId)
                .ForeignKey("dbo.Tracks", t => t.Track_TrackId)
                .Index(t => t.Track_TrackId);
            
            CreateTable(
                "dbo.Genres",
                c => new
                    {
                        GenreId = c.Int(nullable: false, identity: true),
                        GenreName = c.String(),
                        GenreSpotifyName = c.String(),
                        Album_AlbumId = c.Int(),
                        Playlist_PlaylistId = c.Int(),
                    })
                .PrimaryKey(t => t.GenreId)
                .ForeignKey("dbo.Albums", t => t.Album_AlbumId)
                .ForeignKey("dbo.Playlists", t => t.Playlist_PlaylistId)
                .Index(t => t.Album_AlbumId)
                .Index(t => t.Playlist_PlaylistId);
            
            CreateTable(
                "dbo.Tracks",
                c => new
                    {
                        TrackId = c.Int(nullable: false, identity: true),
                        TrackSpotifyId = c.String(),
                        TrackSpotifyUrl = c.String(),
                        TrackName = c.String(),
                        TrackAlbumId = c.Int(nullable: false),
                        TrackAlbumSpotifyId = c.String(),
                        TrackValence = c.Double(nullable: false),
                        TrackEnergy = c.Double(nullable: false),
                        TrackDanceability = c.Double(nullable: false),
                        TrackLoudness = c.Double(nullable: false),
                        TrackPopularity = c.Int(nullable: false),
                        TrackTempo = c.Double(nullable: false),
                        TrackDurationInMs = c.Int(nullable: false),
                        TrackIsInMajorKey = c.Boolean(nullable: false),
                        TrackChecked = c.Boolean(nullable: false),
                        Album_AlbumId = c.Int(),
                        Listener_ListenerId = c.Int(),
                        Playlist_PlaylistId = c.Int(),
                    })
                .PrimaryKey(t => t.TrackId)
                .ForeignKey("dbo.Albums", t => t.Album_AlbumId)
                .ForeignKey("dbo.Listeners", t => t.Listener_ListenerId)
                .ForeignKey("dbo.Playlists", t => t.Playlist_PlaylistId)
                .Index(t => t.Album_AlbumId)
                .Index(t => t.Listener_ListenerId)
                .Index(t => t.Playlist_PlaylistId);
            
            CreateTable(
                "dbo.Listeners",
                c => new
                    {
                        ListenerId = c.Int(nullable: false, identity: true),
                        ScreenName = c.String(maxLength: 30),
                        FirstName = c.String(),
                        LastName = c.String(),
                        AccessToken = c.String(),
                        RefreshToken = c.String(),
                        UserGuid = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ListenerId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserGuid)
                .Index(t => t.ScreenName, unique: true)
                .Index(t => t.UserGuid, unique: true);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Moods",
                c => new
                    {
                        MoodId = c.Int(nullable: false, identity: true),
                        MoodName = c.String(),
                        MoodEnergyMinimum = c.Double(nullable: false),
                        MoodEnergyMaximum = c.Double(nullable: false),
                        MoodEnergyTarget = c.Double(nullable: false),
                        MoodAcousticnessMinimum = c.Double(nullable: false),
                        MoodAcousticnessMaximum = c.Double(nullable: false),
                        MoodAcousticnessTarget = c.Double(nullable: false),
                        MoodSpeechinessMinimum = c.Double(nullable: false),
                        MoodSpeechinessMaximum = c.Double(nullable: false),
                        MoodSpeechinessTarget = c.Double(nullable: false),
                        MoodInstrumentalnessMinimum = c.Double(nullable: false),
                        MoodInstrumentalnessMaximum = c.Double(nullable: false),
                        MoodInstrumentalnessTarget = c.Double(nullable: false),
                        MoodLivenessMinimum = c.Double(nullable: false),
                        MoodLivenessMaximum = c.Double(nullable: false),
                        MoodLivenessTarget = c.Double(nullable: false),
                        MoodDanceabilityMinimum = c.Double(nullable: false),
                        MoodDanceabilityMaximum = c.Double(nullable: false),
                        MoodDanceabilityTarget = c.Double(nullable: false),
                        MoodLoudnessMinimum = c.Double(nullable: false),
                        MoodLoudnessMaximum = c.Double(nullable: false),
                        MoodLoudnessTarget = c.Double(nullable: false),
                        MoodTempoMinimum = c.Double(nullable: false),
                        MoodTempoMaximum = c.Double(nullable: false),
                        MoodTempoTarget = c.Double(nullable: false),
                        MoodValenceMinimum = c.Double(nullable: false),
                        MoodValenceMaximum = c.Double(nullable: false),
                        MoodValenceTarget = c.Double(nullable: false),
                        IsInMajorKeyMood = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MoodId);
            
            CreateTable(
                "dbo.Playlists",
                c => new
                    {
                        PlaylistId = c.Int(nullable: false, identity: true),
                        IsPrivate = c.Boolean(nullable: false),
                        PlaylistName = c.String(),
                        PlaylistMood = c.Int(nullable: false),
                        GenreWeightPercentage = c.Int(nullable: false),
                        PopularityTarget = c.Int(nullable: false),
                        DynamicTracksOnly = c.Boolean(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PlaylistId)
                .ForeignKey("dbo.Listeners", t => t.CreatedBy, cascadeDelete: true)
                .ForeignKey("dbo.Moods", t => t.PlaylistMood, cascadeDelete: true)
                .Index(t => t.PlaylistMood)
                .Index(t => t.CreatedBy);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.ArtistAlbums",
                c => new
                    {
                        Artist_ArtistId = c.Int(nullable: false),
                        Album_AlbumId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Artist_ArtistId, t.Album_AlbumId })
                .ForeignKey("dbo.Artists", t => t.Artist_ArtistId, cascadeDelete: true)
                .ForeignKey("dbo.Albums", t => t.Album_AlbumId, cascadeDelete: true)
                .Index(t => t.Artist_ArtistId)
                .Index(t => t.Album_AlbumId);
            
            CreateTable(
                "dbo.TrackGenres",
                c => new
                    {
                        Track_TrackId = c.Int(nullable: false),
                        Genre_GenreId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Track_TrackId, t.Genre_GenreId })
                .ForeignKey("dbo.Tracks", t => t.Track_TrackId, cascadeDelete: true)
                .ForeignKey("dbo.Genres", t => t.Genre_GenreId, cascadeDelete: true)
                .Index(t => t.Track_TrackId)
                .Index(t => t.Genre_GenreId);
            
            CreateTable(
                "dbo.GenreArtists",
                c => new
                    {
                        Genre_GenreId = c.Int(nullable: false),
                        Artist_ArtistId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Genre_GenreId, t.Artist_ArtistId })
                .ForeignKey("dbo.Genres", t => t.Genre_GenreId, cascadeDelete: true)
                .ForeignKey("dbo.Artists", t => t.Artist_ArtistId, cascadeDelete: true)
                .Index(t => t.Genre_GenreId)
                .Index(t => t.Artist_ArtistId);
            
            CreateTable(
                "dbo.ListenerArtists",
                c => new
                    {
                        Listener_ListenerId = c.Int(nullable: false),
                        Artist_ArtistId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Listener_ListenerId, t.Artist_ArtistId })
                .ForeignKey("dbo.Listeners", t => t.Listener_ListenerId, cascadeDelete: true)
                .ForeignKey("dbo.Artists", t => t.Artist_ArtistId, cascadeDelete: true)
                .Index(t => t.Listener_ListenerId)
                .Index(t => t.Artist_ArtistId);
            
            CreateTable(
                "dbo.ListenerGenres",
                c => new
                    {
                        Listener_ListenerId = c.Int(nullable: false),
                        Genre_GenreId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Listener_ListenerId, t.Genre_GenreId })
                .ForeignKey("dbo.Listeners", t => t.Listener_ListenerId, cascadeDelete: true)
                .ForeignKey("dbo.Genres", t => t.Genre_GenreId, cascadeDelete: true)
                .Index(t => t.Listener_ListenerId)
                .Index(t => t.Genre_GenreId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Tracks", "Playlist_PlaylistId", "dbo.Playlists");
            DropForeignKey("dbo.Genres", "Playlist_PlaylistId", "dbo.Playlists");
            DropForeignKey("dbo.Playlists", "PlaylistMood", "dbo.Moods");
            DropForeignKey("dbo.Playlists", "CreatedBy", "dbo.Listeners");
            DropForeignKey("dbo.Genres", "Album_AlbumId", "dbo.Albums");
            DropForeignKey("dbo.Listeners", "UserGuid", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Tracks", "Listener_ListenerId", "dbo.Listeners");
            DropForeignKey("dbo.ListenerGenres", "Genre_GenreId", "dbo.Genres");
            DropForeignKey("dbo.ListenerGenres", "Listener_ListenerId", "dbo.Listeners");
            DropForeignKey("dbo.ListenerArtists", "Artist_ArtistId", "dbo.Artists");
            DropForeignKey("dbo.ListenerArtists", "Listener_ListenerId", "dbo.Listeners");
            DropForeignKey("dbo.GenreArtists", "Artist_ArtistId", "dbo.Artists");
            DropForeignKey("dbo.GenreArtists", "Genre_GenreId", "dbo.Genres");
            DropForeignKey("dbo.TrackGenres", "Genre_GenreId", "dbo.Genres");
            DropForeignKey("dbo.TrackGenres", "Track_TrackId", "dbo.Tracks");
            DropForeignKey("dbo.Artists", "Track_TrackId", "dbo.Tracks");
            DropForeignKey("dbo.Tracks", "Album_AlbumId", "dbo.Albums");
            DropForeignKey("dbo.ArtistAlbums", "Album_AlbumId", "dbo.Albums");
            DropForeignKey("dbo.ArtistAlbums", "Artist_ArtistId", "dbo.Artists");
            DropIndex("dbo.ListenerGenres", new[] { "Genre_GenreId" });
            DropIndex("dbo.ListenerGenres", new[] { "Listener_ListenerId" });
            DropIndex("dbo.ListenerArtists", new[] { "Artist_ArtistId" });
            DropIndex("dbo.ListenerArtists", new[] { "Listener_ListenerId" });
            DropIndex("dbo.GenreArtists", new[] { "Artist_ArtistId" });
            DropIndex("dbo.GenreArtists", new[] { "Genre_GenreId" });
            DropIndex("dbo.TrackGenres", new[] { "Genre_GenreId" });
            DropIndex("dbo.TrackGenres", new[] { "Track_TrackId" });
            DropIndex("dbo.ArtistAlbums", new[] { "Album_AlbumId" });
            DropIndex("dbo.ArtistAlbums", new[] { "Artist_ArtistId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Playlists", new[] { "CreatedBy" });
            DropIndex("dbo.Playlists", new[] { "PlaylistMood" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Listeners", new[] { "UserGuid" });
            DropIndex("dbo.Listeners", new[] { "ScreenName" });
            DropIndex("dbo.Tracks", new[] { "Playlist_PlaylistId" });
            DropIndex("dbo.Tracks", new[] { "Listener_ListenerId" });
            DropIndex("dbo.Tracks", new[] { "Album_AlbumId" });
            DropIndex("dbo.Genres", new[] { "Playlist_PlaylistId" });
            DropIndex("dbo.Genres", new[] { "Album_AlbumId" });
            DropIndex("dbo.Artists", new[] { "Track_TrackId" });
            DropTable("dbo.ListenerGenres");
            DropTable("dbo.ListenerArtists");
            DropTable("dbo.GenreArtists");
            DropTable("dbo.TrackGenres");
            DropTable("dbo.ArtistAlbums");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Playlists");
            DropTable("dbo.Moods");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Listeners");
            DropTable("dbo.Tracks");
            DropTable("dbo.Genres");
            DropTable("dbo.Artists");
            DropTable("dbo.Albums");
        }
    }
}
