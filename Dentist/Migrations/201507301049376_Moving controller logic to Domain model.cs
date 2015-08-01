namespace Dentist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MovingcontrollerlogictoDomainmodel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DoctorPractices", "Doctor_Id", "dbo.Doctors");
            DropForeignKey("dbo.DoctorPractices", "Practice_Id", "dbo.Practices");
            DropIndex("dbo.DoctorPractices", new[] { "Doctor_Id" });
            DropIndex("dbo.DoctorPractices", new[] { "Practice_Id" });
            AddColumn("dbo.Doctors", "Practice_Id", c => c.Int());
            CreateIndex("dbo.Doctors", "Practice_Id");
            AddForeignKey("dbo.Doctors", "Practice_Id", "dbo.Practices", "Id");
            DropTable("dbo.DoctorPractices");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.DoctorPractices",
                c => new
                    {
                        Doctor_Id = c.Int(nullable: false),
                        Practice_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Doctor_Id, t.Practice_Id });
            
            DropForeignKey("dbo.Doctors", "Practice_Id", "dbo.Practices");
            DropIndex("dbo.Doctors", new[] { "Practice_Id" });
            DropColumn("dbo.Doctors", "Practice_Id");
            CreateIndex("dbo.DoctorPractices", "Practice_Id");
            CreateIndex("dbo.DoctorPractices", "Doctor_Id");
            AddForeignKey("dbo.DoctorPractices", "Practice_Id", "dbo.Practices", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DoctorPractices", "Doctor_Id", "dbo.Doctors", "Id", cascadeDelete: true);
        }
    }
}
