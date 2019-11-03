namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlaylistTrackCount1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Playlists", "TrackCount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Playlists", "TrackCount", c => c.Int());
        }
    }
}
