namespace Tarbya.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addStudentEventTable1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StudentEvents",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        studentID = c.Int(nullable: false),
                        eventID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Events", t => t.eventID, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.studentID, cascadeDelete: true)
                .Index(t => t.studentID)
                .Index(t => t.eventID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StudentEvents", "studentID", "dbo.Students");
            DropForeignKey("dbo.StudentEvents", "eventID", "dbo.Events");
            DropIndex("dbo.StudentEvents", new[] { "eventID" });
            DropIndex("dbo.StudentEvents", new[] { "studentID" });
            DropTable("dbo.StudentEvents");
        }
    }
}
