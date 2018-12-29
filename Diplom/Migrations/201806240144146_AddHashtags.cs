namespace Diplom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHashtags : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Hashtags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.Tweets", "Data");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tweets", "Data", c => c.DateTime(nullable: false));
            DropTable("dbo.Hashtags");
        }
    }
}
