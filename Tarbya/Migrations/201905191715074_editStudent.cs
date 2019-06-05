namespace Tarbya.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editStudent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "facultyID", c => c.Int(nullable: false));
            AddColumn("dbo.Students", "yearID", c => c.Int(nullable: false));
            AddColumn("dbo.Students", "educationalQualificationID", c => c.Int(nullable: false));
            CreateIndex("dbo.Students", "facultyID");
            CreateIndex("dbo.Students", "yearID");
            CreateIndex("dbo.Students", "educationalQualificationID");
            AddForeignKey("dbo.Students", "educationalQualificationID", "dbo.EducationalQualifications", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Students", "facultyID", "dbo.Faculties", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Students", "yearID", "dbo.Years", "ID", cascadeDelete: true);
            DropColumn("dbo.Students", "faculty");
            DropColumn("dbo.Students", "year");
            DropColumn("dbo.Students", "educationalQualification");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Students", "educationalQualification", c => c.String());
            AddColumn("dbo.Students", "year", c => c.String());
            AddColumn("dbo.Students", "faculty", c => c.String());
            DropForeignKey("dbo.Students", "yearID", "dbo.Years");
            DropForeignKey("dbo.Students", "facultyID", "dbo.Faculties");
            DropForeignKey("dbo.Students", "educationalQualificationID", "dbo.EducationalQualifications");
            DropIndex("dbo.Students", new[] { "educationalQualificationID" });
            DropIndex("dbo.Students", new[] { "yearID" });
            DropIndex("dbo.Students", new[] { "facultyID" });
            DropColumn("dbo.Students", "educationalQualificationID");
            DropColumn("dbo.Students", "yearID");
            DropColumn("dbo.Students", "facultyID");
        }
    }
}
