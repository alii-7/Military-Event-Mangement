namespace Tarbya.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editModelsFolder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EducationalQualifications", "educationalQualificationName", c => c.String());
            AddColumn("dbo.Years", "yearStatusName", c => c.String());
            DropColumn("dbo.EducationalQualifications", "educationalQualification");
            DropColumn("dbo.Years", "year");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Years", "year", c => c.String());
            AddColumn("dbo.EducationalQualifications", "educationalQualification", c => c.String());
            DropColumn("dbo.Years", "yearStatusName");
            DropColumn("dbo.EducationalQualifications", "educationalQualificationName");
        }
    }
}
