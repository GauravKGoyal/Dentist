namespace Dentist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class doctormodelupdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Registrations", "Id", "dbo.People");
            DropForeignKey("dbo.People", "Registration_Id", "dbo.Registrations");
            DropIndex("dbo.Registrations", new[] { "Id" });
            DropPrimaryKey("dbo.Registrations");
            AddColumn("dbo.People", "Registration_Id", c => c.Int());
            AlterColumn("dbo.Registrations", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Registrations", "Id");
            CreateIndex("dbo.People", "Registration_Id");
            AddForeignKey("dbo.People", "Registration_Id", "dbo.Registrations", "Id");
            DropColumn("dbo.Registrations", "DoctorId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Registrations", "DoctorId", c => c.Int(nullable: false));
            DropForeignKey("dbo.People", "Registration_Id", "dbo.Registrations");
            DropIndex("dbo.People", new[] { "Registration_Id" });
            DropPrimaryKey("dbo.Registrations");
            AlterColumn("dbo.Registrations", "Id", c => c.Int(nullable: false));
            DropColumn("dbo.People", "Registration_Id");
            AddPrimaryKey("dbo.Registrations", "Id");
            CreateIndex("dbo.Registrations", "Id");
            AddForeignKey("dbo.People", "Registration_Id", "dbo.Registrations", "Id");
            AddForeignKey("dbo.Registrations", "Id", "dbo.People", "Id");
        }
    }
}
