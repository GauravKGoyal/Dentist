namespace Dentist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _5 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Doctors", "Practice_Id", "dbo.Practices");
            DropIndex("dbo.Doctors", new[] { "Practice_Id" });
            CreateTable(
                "dbo.DoctorPractices",
                c => new
                    {
                        Doctor_Id = c.Int(nullable: false),
                        Practice_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Doctor_Id, t.Practice_Id })
                .ForeignKey("dbo.Doctors", t => t.Doctor_Id, cascadeDelete: true)
                .ForeignKey("dbo.Practices", t => t.Practice_Id, cascadeDelete: true)
                .Index(t => t.Doctor_Id)
                .Index(t => t.Practice_Id);
            
            DropColumn("dbo.Doctors", "Practice_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Doctors", "Practice_Id", c => c.Int());
            DropForeignKey("dbo.DoctorPractices", "Practice_Id", "dbo.Practices");
            DropForeignKey("dbo.DoctorPractices", "Doctor_Id", "dbo.Doctors");
            DropIndex("dbo.DoctorPractices", new[] { "Practice_Id" });
            DropIndex("dbo.DoctorPractices", new[] { "Doctor_Id" });
            DropTable("dbo.DoctorPractices");
            CreateIndex("dbo.Doctors", "Practice_Id");
            AddForeignKey("dbo.Doctors", "Practice_Id", "dbo.Practices", "Id");
        }
    }
}
