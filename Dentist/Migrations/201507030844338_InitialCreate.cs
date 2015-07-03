namespace Dentist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddressLine1 = c.String(),
                        Suburb = c.String(),
                        City = c.String(),
                        Country = c.String(),
                        PinCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Practices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        PracticeTagline = c.String(),
                        Phone = c.String(),
                        Email = c.String(),
                        Website = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        AddressId = c.Int(nullable: false),
                        Color = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.AddressId)
                .Index(t => t.AddressId);
            
            CreateTable(
                "dbo.DailyAvailabilities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DayOfWeek = c.Int(nullable: false),
                        DoctorId = c.Int(nullable: false),
                        PracticeId = c.Int(nullable: false),
                        IsWorking = c.Boolean(nullable: false),
                        StartTime1 = c.DateTime(),
                        EndTime1 = c.DateTime(),
                        StartTime2 = c.DateTime(),
                        EndTime2 = c.DateTime(),
                        StartTime3 = c.DateTime(),
                        EndTime3 = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Doctors", t => t.DoctorId)
                .ForeignKey("dbo.Practices", t => t.PracticeId)
                .Index(t => t.DoctorId)
                .Index(t => t.PracticeId);
            
            CreateTable(
                "dbo.Doctors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Color = c.String(),
                        Title = c.Int(nullable: false),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Email = c.String(),
                        DateOfBirth = c.DateTime(),
                        Phone = c.String(),
                        PersonRole = c.Int(nullable: false),
                        AddressId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.AddressId)
                .Index(t => t.AddressId);
            
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartDateTime = c.DateTime(nullable: false),
                        EndDateTime = c.DateTime(nullable: false),
                        DoctorId = c.Int(nullable: false),
                        PatientId = c.Int(nullable: false),
                        PracticeId = c.Int(nullable: false),
                        Description = c.String(),
                        AppointmentStatus = c.Int(nullable: false),
                        RecurrenceRule = c.String(),
                        RecurrenceException = c.String(),
                        IsBreak = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Doctors", t => t.DoctorId)
                .ForeignKey("dbo.Paitients", t => t.PatientId)
                .ForeignKey("dbo.Practices", t => t.PracticeId)
                .Index(t => t.DoctorId)
                .Index(t => t.PatientId)
                .Index(t => t.PracticeId);
            
            CreateTable(
                "dbo.Paitients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PracticeId = c.Int(nullable: false),
                        Title = c.Int(nullable: false),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Email = c.String(),
                        DateOfBirth = c.DateTime(),
                        Phone = c.String(),
                        PersonRole = c.Int(nullable: false),
                        AddressId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.AddressId)
                .ForeignKey("dbo.Practices", t => t.PracticeId)
                .Index(t => t.PracticeId)
                .Index(t => t.AddressId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.DailyAvailabilities", "PracticeId", "dbo.Practices");
            DropForeignKey("dbo.DoctorPractices", "Practice_Id", "dbo.Practices");
            DropForeignKey("dbo.DoctorPractices", "Doctor_Id", "dbo.Doctors");
            DropForeignKey("dbo.DailyAvailabilities", "DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.Appointments", "PracticeId", "dbo.Practices");
            DropForeignKey("dbo.Appointments", "PatientId", "dbo.Paitients");
            DropForeignKey("dbo.Paitients", "PracticeId", "dbo.Practices");
            DropForeignKey("dbo.Paitients", "AddressId", "dbo.Addresses");
            DropForeignKey("dbo.Appointments", "DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.Doctors", "AddressId", "dbo.Addresses");
            DropForeignKey("dbo.Practices", "AddressId", "dbo.Addresses");
            DropIndex("dbo.DoctorPractices", new[] { "Practice_Id" });
            DropIndex("dbo.DoctorPractices", new[] { "Doctor_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Paitients", new[] { "AddressId" });
            DropIndex("dbo.Paitients", new[] { "PracticeId" });
            DropIndex("dbo.Appointments", new[] { "PracticeId" });
            DropIndex("dbo.Appointments", new[] { "PatientId" });
            DropIndex("dbo.Appointments", new[] { "DoctorId" });
            DropIndex("dbo.Doctors", new[] { "AddressId" });
            DropIndex("dbo.DailyAvailabilities", new[] { "PracticeId" });
            DropIndex("dbo.DailyAvailabilities", new[] { "DoctorId" });
            DropIndex("dbo.Practices", new[] { "AddressId" });
            DropTable("dbo.DoctorPractices");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Paitients");
            DropTable("dbo.Appointments");
            DropTable("dbo.Doctors");
            DropTable("dbo.DailyAvailabilities");
            DropTable("dbo.Practices");
            DropTable("dbo.Addresses");
        }
    }
}
