namespace Diplom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTweets : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tweets", "Data", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Tweets", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Tweets", "DogName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Tweets", "Text", c => c.String(nullable: false));
            AlterColumn("dbo.Tweets", "ImageUrl", c => c.String(nullable: false));
            AlterColumn("dbo.Tweets", "DataInfo", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tweets", "DataInfo", c => c.String());
            AlterColumn("dbo.Tweets", "ImageUrl", c => c.String());
            AlterColumn("dbo.Tweets", "Text", c => c.String());
            AlterColumn("dbo.Tweets", "DogName", c => c.String(maxLength: 50));
            AlterColumn("dbo.Tweets", "Name", c => c.String(maxLength: 50));
            DropColumn("dbo.Tweets", "Data");
        }
    }
}
