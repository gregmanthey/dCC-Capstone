namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlaylistUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Playlists", "IsPrivate", c => c.Boolean(nullable: false));
            AddColumn("dbo.Playlists", "DynamicTracksOnly", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Playlists", "DynamicTracksOnly");
            DropColumn("dbo.Playlists", "IsPrivate");
        }
    }
}
