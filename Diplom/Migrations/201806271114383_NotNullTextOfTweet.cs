namespace Diplom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotNullTextOfTweet : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tweets", "Text", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tweets", "Text", c => c.String(nullable: false));
        }
    }
}
