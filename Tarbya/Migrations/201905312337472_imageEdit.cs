namespace Tarbya.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class imageEdit : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Students", "image", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Students", "image", c => c.String());
        }
    }
}
