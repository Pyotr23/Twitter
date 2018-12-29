namespace Diplom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTypeOfDogNameUser : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Users");
            AlterColumn("dbo.Users", "DogName", c => c.String(nullable: false, maxLength: 50));
            AddPrimaryKey("dbo.Users", "DogName");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Users");
            AlterColumn("dbo.Users", "DogName", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Users", "DogName");
        }
    }
}
