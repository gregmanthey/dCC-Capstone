namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlaylistMoodIntNullable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Playlists", "PlaylistMood", "dbo.Moods");
            DropIndex("dbo.Playlists", new[] { "PlaylistMood" });
            AlterColumn("dbo.Playlists", "PlaylistMood", c => c.Int());
            CreateIndex("dbo.Playlists", "PlaylistMood");
            AddForeignKey("dbo.Playlists", "PlaylistMood", "dbo.Moods", "MoodId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Playlists", "PlaylistMood", "dbo.Moods");
            DropIndex("dbo.Playlists", new[] { "PlaylistMood" });
            AlterColumn("dbo.Playlists", "PlaylistMood", c => c.Int(nullable: false));
            CreateIndex("dbo.Playlists", "PlaylistMood");
            AddForeignKey("dbo.Playlists", "PlaylistMood", "dbo.Moods", "MoodId", cascadeDelete: true);
        }
    }
}
