namespace Passion_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class recipe : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Recipes",
                c => new
                    {
                        RecipeId = c.Int(nullable: false, identity: true),
                        RecipeName = c.String(),
                        RecipeIngredient = c.String(),
                        RecipeInstruction = c.String(),
                    })
                .PrimaryKey(t => t.RecipeId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Recipes");
        }
    }
}
