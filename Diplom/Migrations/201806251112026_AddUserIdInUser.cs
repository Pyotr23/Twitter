namespace Diplom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserIdInUser : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Users");
            AddColumn("dbo.Users", "UserId", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Users", "DogName", c => c.String(nullable: false, maxLength: 50));
            AddPrimaryKey("dbo.Users", "UserId");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Users");
            AlterColumn("dbo.Users", "DogName", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.Users", "UserId");
            AddPrimaryKey("dbo.Users", "DogName");
        }
    }
}
