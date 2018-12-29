namespace Diplom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeSizeUrlOfUser : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "Image", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Image", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
