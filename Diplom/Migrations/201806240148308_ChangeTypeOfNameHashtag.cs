namespace Diplom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTypeOfNameHashtag : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Hashtags", "Name", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Hashtags", "Name", c => c.String());
        }
    }
}
