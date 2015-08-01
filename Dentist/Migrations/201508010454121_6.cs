namespace Dentist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _6 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Doctors", "Context_RequireUniqueEmail");
            DropColumn("dbo.Paitients", "Context_RequireUniqueEmail");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Paitients", "Context_RequireUniqueEmail", c => c.Boolean(nullable: false));
            AddColumn("dbo.Doctors", "Context_RequireUniqueEmail", c => c.Boolean(nullable: false));
        }
    }
}
