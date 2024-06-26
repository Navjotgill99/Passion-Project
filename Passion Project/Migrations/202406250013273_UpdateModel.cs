namespace Passion_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "UserId", "dbo.Users");
            DropIndex("dbo.Comments", new[] { "UserId" });
            RenameColumn(table: "dbo.Comments", name: "UserId", newName: "Id");
            AddColumn("dbo.Recipes", "Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Comments", "Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Comments", "Id");
            CreateIndex("dbo.Recipes", "Id");
            AddForeignKey("dbo.Recipes", "Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Comments", "Id", "dbo.AspNetUsers", "Id");
      
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
            DropForeignKey("dbo.Comments", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Recipes", "Id", "dbo.AspNetUsers");
            DropIndex("dbo.Recipes", new[] { "Id" });
            DropIndex("dbo.Comments", new[] { "Id" });
            AlterColumn("dbo.Comments", "Id", c => c.Int(nullable: false));
            DropColumn("dbo.Recipes", "Id");
            RenameColumn(table: "dbo.Comments", name: "Id", newName: "UserId");
            CreateIndex("dbo.Comments", "UserId");
            AddForeignKey("dbo.Comments", "UserId", "dbo.Users", "UserId", cascadeDelete: true);
        }
    }
}
