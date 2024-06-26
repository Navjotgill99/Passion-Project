namespace Passion_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveUserModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "UserId", "dbo.Users");
            DropIndex("dbo.Comments", new[] { "UserId" });
            AddColumn("dbo.Recipes", "UserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Comments", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Comments", "UserId");
            CreateIndex("dbo.Recipes", "UserId");
            AddForeignKey("dbo.Recipes", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Comments", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            AddColumn("dbo.Recipes", "RecipeAuthor", c => c.String());
            DropForeignKey("dbo.Comments", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Recipes", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Recipes", new[] { "UserId" });
            DropIndex("dbo.Comments", new[] { "UserId" });
            AlterColumn("dbo.Comments", "UserId", c => c.Int(nullable: false));
            DropColumn("dbo.Recipes", "UserId");
            CreateIndex("dbo.Comments", "UserId");
            AddForeignKey("dbo.Comments", "UserId", "dbo.Users", "UserId", cascadeDelete: true);
        }
    }
}
