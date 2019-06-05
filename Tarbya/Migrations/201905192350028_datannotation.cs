namespace Tarbya.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class datannotation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Students", "firstName", c => c.String(nullable: false));
            AlterColumn("dbo.Students", "secondName", c => c.String(nullable: false));
            AlterColumn("dbo.Students", "thirdName", c => c.String(nullable: false));
            AlterColumn("dbo.Students", "fourthName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Students", "fourthName", c => c.String());
            AlterColumn("dbo.Students", "thirdName", c => c.String());
            AlterColumn("dbo.Students", "secondName", c => c.String());
            AlterColumn("dbo.Students", "firstName", c => c.String());
        }
    }
}
