namespace Tarbya.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class religion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "religion", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Students", "religion");
        }
    }
}
