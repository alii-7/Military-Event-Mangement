namespace Tarbya.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addEventTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        eventName = c.String(),
                        dateOFEvent = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Events");
        }
    }
}
