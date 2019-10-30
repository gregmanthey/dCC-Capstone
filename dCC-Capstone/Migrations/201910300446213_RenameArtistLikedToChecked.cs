namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameArtistLikedToChecked : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Artists", "Checked", c => c.Boolean(nullable: false));
            DropColumn("dbo.Artists", "Liked");
            DropColumn("dbo.Artists", "Disliked");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Artists", "Disliked", c => c.Boolean(nullable: false));
            AddColumn("dbo.Artists", "Liked", c => c.Boolean(nullable: false));
            DropColumn("dbo.Artists", "Checked");
        }
    }
}
