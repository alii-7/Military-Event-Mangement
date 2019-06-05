namespace Tarbya.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedBlockedField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "Blocked", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Students", "Blocked");
        }
    }
}
