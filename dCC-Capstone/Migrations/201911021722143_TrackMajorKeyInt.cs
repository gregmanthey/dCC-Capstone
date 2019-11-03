namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TrackMajorKeyInt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tracks", "TrackIsInMajorKey", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tracks", "TrackIsInMajorKey", c => c.Boolean(nullable: false));
        }
    }
}
