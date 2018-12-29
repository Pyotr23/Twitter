namespace Diplom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUsers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        DogName = c.String(nullable: false, maxLength: 128),
                        FIO = c.String(nullable: false, maxLength: 50),
                        Followers = c.Int(nullable: false),
                        Follow = c.Int(nullable: false),
                        Image = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.DogName);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
        }
    }
}
