namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LikeDislikeArtist : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Artists", "Liked", c => c.Boolean(nullable: false));
            AddColumn("dbo.Artists", "Disliked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Artists", "Disliked");
            DropColumn("dbo.Artists", "Liked");
        }
    }
}
