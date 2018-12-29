namespace Diplom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReturnTextOfTweet : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tweets", "Text", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tweets", "Text");
        }
    }
}
