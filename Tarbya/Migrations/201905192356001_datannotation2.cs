namespace Tarbya.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class datannotation2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Students", "gender", c => c.String(nullable: false));
            AlterColumn("dbo.Students", "phoneNumber", c => c.String(nullable: false));
            AlterColumn("dbo.Students", "address", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Students", "address", c => c.String());
            AlterColumn("dbo.Students", "phoneNumber", c => c.String());
            AlterColumn("dbo.Students", "gender", c => c.String());
        }
    }
}
