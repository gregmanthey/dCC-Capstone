namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TrackListeners : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tracks", "Listener_ListenerId", "dbo.Listeners");
            DropIndex("dbo.Tracks", new[] { "Listener_ListenerId" });
            CreateTable(
                "dbo.ListenerTracks",
                c => new
                    {
                        Listener_ListenerId = c.Int(nullable: false),
                        Track_TrackId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Listener_ListenerId, t.Track_TrackId })
                .ForeignKey("dbo.Listeners", t => t.Listener_ListenerId, cascadeDelete: true)
                .ForeignKey("dbo.Tracks", t => t.Track_TrackId, cascadeDelete: true)
                .Index(t => t.Listener_ListenerId)
                .Index(t => t.Track_TrackId);
            
            DropColumn("dbo.Tracks", "Listener_ListenerId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tracks", "Listener_ListenerId", c => c.Int());
            DropForeignKey("dbo.ListenerTracks", "Track_TrackId", "dbo.Tracks");
            DropForeignKey("dbo.ListenerTracks", "Listener_ListenerId", "dbo.Listeners");
            DropIndex("dbo.ListenerTracks", new[] { "Track_TrackId" });
            DropIndex("dbo.ListenerTracks", new[] { "Listener_ListenerId" });
            DropTable("dbo.ListenerTracks");
            CreateIndex("dbo.Tracks", "Listener_ListenerId");
            AddForeignKey("dbo.Tracks", "Listener_ListenerId", "dbo.Listeners", "ListenerId");
        }
    }
}
