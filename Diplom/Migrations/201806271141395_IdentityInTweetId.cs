namespace Diplom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdentityInTweetId : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Tweets");
            AlterColumn("dbo.Tweets", "ID", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.Tweets", "ID");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Tweets");
            AlterColumn("dbo.Tweets", "ID", c => c.Long(nullable: false));
            AddPrimaryKey("dbo.Tweets", "ID");
        }
    }
}
