namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddArtistUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Artists", "ArtistSpotifyUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Artists", "ArtistSpotifyUrl");
        }
    }
}
