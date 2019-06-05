namespace Tarbya.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class datannotation1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Students", "socialSecurityNumber", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Students", "socialSecurityNumber", c => c.String());
        }
    }
}
