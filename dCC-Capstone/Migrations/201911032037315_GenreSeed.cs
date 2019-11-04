namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GenreSeed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Genres", "IsSpotifyGenreSeed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Genres", "IsSpotifyGenreSeed");
        }
    }
}
