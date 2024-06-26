namespace Passion_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class recipeusername : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Recipes", "UserName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Recipes", "UserName");
        }
    }
}
