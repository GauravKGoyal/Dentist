namespace Dentist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CalernderSettings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CalenderSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DayStartTime = c.DateTime(nullable: false),
                        DayEndTime = c.DateTime(nullable: false),
                        WorkWeekStartTime = c.DateTime(nullable: false),
                        WorkWeekEndTime = c.DateTime(nullable: false),
                        WorkWeekStartDay = c.Int(nullable: false),
                        WorkWeekEndDay = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CalenderSettings");
        }
    }
}
