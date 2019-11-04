namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SingleAlbumArtist : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.GenreArtists", newName: "ArtistGenres");
            DropForeignKey("dbo.ArtistAlbums", "Artist_ArtistId", "dbo.Artists");
            DropForeignKey("dbo.ArtistAlbums", "Album_AlbumId", "dbo.Albums");
            DropIndex("dbo.ArtistAlbums", new[] { "Artist_ArtistId" });
            DropIndex("dbo.ArtistAlbums", new[] { "Album_AlbumId" });
            DropPrimaryKey("dbo.ArtistGenres");
            AddColumn("dbo.Albums", "AlbumArtistId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.ArtistGenres", new[] { "Artist_ArtistId", "Genre_GenreId" });
            CreateIndex("dbo.Albums", "AlbumArtistId");
            AddForeignKey("dbo.Albums", "AlbumArtistId", "dbo.Artists", "ArtistId", cascadeDelete: true);
            DropTable("dbo.ArtistAlbums");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ArtistAlbums",
                c => new
                    {
                        Artist_ArtistId = c.Int(nullable: false),
                        Album_AlbumId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Artist_ArtistId, t.Album_AlbumId });
            
            DropForeignKey("dbo.Albums", "AlbumArtistId", "dbo.Artists");
            DropIndex("dbo.Albums", new[] { "AlbumArtistId" });
            DropPrimaryKey("dbo.ArtistGenres");
            DropColumn("dbo.Albums", "AlbumArtistId");
            AddPrimaryKey("dbo.ArtistGenres", new[] { "Genre_GenreId", "Artist_ArtistId" });
            CreateIndex("dbo.ArtistAlbums", "Album_AlbumId");
            CreateIndex("dbo.ArtistAlbums", "Artist_ArtistId");
            AddForeignKey("dbo.ArtistAlbums", "Album_AlbumId", "dbo.Albums", "AlbumId", cascadeDelete: true);
            AddForeignKey("dbo.ArtistAlbums", "Artist_ArtistId", "dbo.Artists", "ArtistId", cascadeDelete: true);
            RenameTable(name: "dbo.ArtistGenres", newName: "GenreArtists");
        }
    }
}
