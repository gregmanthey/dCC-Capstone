namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImageUrls : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Albums", "AlbumImageUrl", c => c.String());
            AddColumn("dbo.Artists", "ArtistImageUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Artists", "ArtistImageUrl");
            DropColumn("dbo.Albums", "AlbumImageUrl");
        }
    }
}
