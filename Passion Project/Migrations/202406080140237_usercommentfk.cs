namespace Passion_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class usercommentfk : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            AddColumn("dbo.Comments", "UserName", c => c.Int(nullable: false));
            CreateIndex("dbo.Comments", "UserName");
            AddForeignKey("dbo.Comments", "UserName", "dbo.Users", "UserId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "UserName", "dbo.Users");
            DropIndex("dbo.Comments", new[] { "UserName" });
            DropColumn("dbo.Comments", "UserName");
            DropTable("dbo.Users");
        }
    }
}
