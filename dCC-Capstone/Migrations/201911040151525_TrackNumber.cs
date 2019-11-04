namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TrackNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tracks", "TrackNumber", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tracks", "TrackNumber");
        }
    }
}
