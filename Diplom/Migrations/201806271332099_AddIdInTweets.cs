namespace Diplom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIdInTweets : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Tweets");
            AddColumn("dbo.Tweets", "Identifier", c => c.Long(nullable: false));
            AlterColumn("dbo.Tweets", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Tweets", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Tweets");
            AlterColumn("dbo.Tweets", "Id", c => c.Long(nullable: false, identity: true));
            DropColumn("dbo.Tweets", "Identifier");
            AddPrimaryKey("dbo.Tweets", "ID");
        }
    }
}
