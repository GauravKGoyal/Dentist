namespace Dentist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class doctordomainupdate2 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.People", name: "Registration_Id", newName: "RegistrationId");
            RenameIndex(table: "dbo.People", name: "IX_Registration_Id", newName: "IX_RegistrationId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.People", name: "IX_RegistrationId", newName: "IX_Registration_Id");
            RenameColumn(table: "dbo.People", name: "RegistrationId", newName: "Registration_Id");
        }
    }
}
