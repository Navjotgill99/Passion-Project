namespace Passion_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dbcontext : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Comments", name: "UserName", newName: "UserId");
            RenameIndex(table: "dbo.Comments", name: "IX_UserName", newName: "IX_UserId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Comments", name: "IX_UserId", newName: "IX_UserName");
            RenameColumn(table: "dbo.Comments", name: "UserId", newName: "UserName");
        }
    }
}
