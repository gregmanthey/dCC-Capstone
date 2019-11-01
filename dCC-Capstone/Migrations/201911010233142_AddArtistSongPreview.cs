namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddArtistSongPreview : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Artists", "ArtistTopTrackPreviewUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Artists", "ArtistTopTrackPreviewUrl");
        }
    }
}
