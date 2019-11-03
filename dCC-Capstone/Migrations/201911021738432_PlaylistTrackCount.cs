namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlaylistTrackCount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Playlists", "TrackCount", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Playlists", "TrackCount");
        }
    }
}
