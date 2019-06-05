namespace Tarbya.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFacultyRelatedTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EducationalQualifications",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        educationalQualification = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Faculties",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        facultyName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Years",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        year = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Years");
            DropTable("dbo.Faculties");
            DropTable("dbo.EducationalQualifications");
        }
    }
}
