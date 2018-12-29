namespace Diplom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteTextFromTweet : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Tweets", "Text");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tweets", "Text", c => c.String(nullable: false));
        }
    }
}
