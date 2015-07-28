namespace Dentist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class settingsdailyavailability : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DailyAvailabilitySettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DayOfWeek = c.Int(nullable: false),
                        IsWorking = c.Boolean(nullable: false),
                        StartTime1 = c.DateTime(),
                        EndTime1 = c.DateTime(),
                        StartTime2 = c.DateTime(),
                        EndTime2 = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DailyAvailabilitySettings");
        }
    }
}
