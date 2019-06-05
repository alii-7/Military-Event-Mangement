namespace Tarbya.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifyEventTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "description", c => c.String(nullable: false));
            AlterColumn("dbo.Events", "eventName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Events", "eventName", c => c.String());
            DropColumn("dbo.Events", "description");
        }
    }
}
