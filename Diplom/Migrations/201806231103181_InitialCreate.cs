namespace Diplom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tweets",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        DogName = c.String(maxLength: 50),
                        Text = c.String(),
                        Data = c.DateTime(nullable: false),
                        ImageUrl = c.String(),
                        DataInfo = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tweets");
        }
    }
}
