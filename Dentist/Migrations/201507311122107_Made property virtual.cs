namespace Dentist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Madepropertyvirtual : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Doctors", "Context_RequireUniqueEmail", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Doctors", "Context_RequireUniqueEmail");
        }
    }
}
