namespace Dentist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DentistProfileupgrade : DbMigration
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
                .ForeignKey("dbo.People", t => t.DoctorId)
                .ForeignKey("dbo.Practices", t => t.PracticeId)
                .Index(t => t.DoctorId)
                .Index(t => t.PracticeId);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.Int(nullable: false),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Email = c.String(),
                        DateOfBirth = c.DateTime(),
                        Phone = c.String(),
                        PersonRole = c.Int(nullable: false),
                        AddressId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        About = c.String(),
                        Color = c.String(),
                        PracticeId = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.AddressId)
                .ForeignKey("dbo.Practices", t => t.PracticeId)
                .Index(t => t.AddressId)
                .Index(t => t.PracticeId);
            
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
                .ForeignKey("dbo.People", t => t.DoctorId)
                .ForeignKey("dbo.People", t => t.PatientId)
                .ForeignKey("dbo.Practices", t => t.PracticeId)
                .Index(t => t.DoctorId)
                .Index(t => t.PatientId)
                .Index(t => t.PracticeId);
            
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(nullable: false, maxLength: 100),
                        ContentType = c.String(nullable: false),
                        Content = c.Binary(nullable: false),
                        FileType = c.Int(nullable: false),
                        CreatedDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Awards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Year = c.Int(nullable: false),
                        DoctorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.DoctorId)
                .Index(t => t.DoctorId);
            
            CreateTable(
                "dbo.Experiences",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FromYear = c.Int(nullable: false),
                        ToYear = c.Int(nullable: false),
                        As = c.String(),
                        At = c.String(),
                        DoctorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.DoctorId)
                .Index(t => t.DoctorId);
            
            CreateTable(
                "dbo.Memberships",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Qualifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        College = c.String(),
                        Year = c.Int(nullable: false),
                        DoctorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.DoctorId)
                .Index(t => t.DoctorId);
            
            CreateTable(
                "dbo.Registrations",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Number = c.String(),
                        College = c.String(),
                        DoctorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Specializations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
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
                "dbo.PersonFiles",
                c => new
                    {
                        Person_Id = c.Int(nullable: false),
                        File_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Person_Id, t.File_Id })
                .ForeignKey("dbo.People", t => t.Person_Id, cascadeDelete: true)
                .ForeignKey("dbo.Files", t => t.File_Id, cascadeDelete: true)
                .Index(t => t.Person_Id)
                .Index(t => t.File_Id);
            
            CreateTable(
                "dbo.MembershipDoctors",
                c => new
                    {
                        Membership_Id = c.Int(nullable: false),
                        Doctor_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Membership_Id, t.Doctor_Id })
                .ForeignKey("dbo.Memberships", t => t.Membership_Id, cascadeDelete: true)
                .ForeignKey("dbo.People", t => t.Doctor_Id, cascadeDelete: true)
                .Index(t => t.Membership_Id)
                .Index(t => t.Doctor_Id);
            
            CreateTable(
                "dbo.DoctorPractices",
                c => new
                    {
                        Doctor_Id = c.Int(nullable: false),
                        Practice_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Doctor_Id, t.Practice_Id })
                .ForeignKey("dbo.People", t => t.Doctor_Id, cascadeDelete: true)
                .ForeignKey("dbo.Practices", t => t.Practice_Id, cascadeDelete: true)
                .Index(t => t.Doctor_Id)
                .Index(t => t.Practice_Id);
            
            CreateTable(
                "dbo.ServiceDoctors",
                c => new
                    {
                        Service_Id = c.Int(nullable: false),
                        Doctor_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Service_Id, t.Doctor_Id })
                .ForeignKey("dbo.Services", t => t.Service_Id, cascadeDelete: true)
                .ForeignKey("dbo.People", t => t.Doctor_Id, cascadeDelete: true)
                .Index(t => t.Service_Id)
                .Index(t => t.Doctor_Id);
            
            CreateTable(
                "dbo.SpecializationDoctors",
                c => new
                    {
                        Specialization_Id = c.Int(nullable: false),
                        Doctor_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Specialization_Id, t.Doctor_Id })
                .ForeignKey("dbo.Specializations", t => t.Specialization_Id, cascadeDelete: true)
                .ForeignKey("dbo.People", t => t.Doctor_Id, cascadeDelete: true)
                .Index(t => t.Specialization_Id)
                .Index(t => t.Doctor_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.DailyAvailabilities", "PracticeId", "dbo.Practices");
            DropForeignKey("dbo.SpecializationDoctors", "Doctor_Id", "dbo.People");
            DropForeignKey("dbo.SpecializationDoctors", "Specialization_Id", "dbo.Specializations");
            DropForeignKey("dbo.ServiceDoctors", "Doctor_Id", "dbo.People");
            DropForeignKey("dbo.ServiceDoctors", "Service_Id", "dbo.Services");
            DropForeignKey("dbo.Registrations", "Id", "dbo.People");
            DropForeignKey("dbo.Qualifications", "DoctorId", "dbo.People");
            DropForeignKey("dbo.DoctorPractices", "Practice_Id", "dbo.Practices");
            DropForeignKey("dbo.DoctorPractices", "Doctor_Id", "dbo.People");
            DropForeignKey("dbo.MembershipDoctors", "Doctor_Id", "dbo.People");
            DropForeignKey("dbo.MembershipDoctors", "Membership_Id", "dbo.Memberships");
            DropForeignKey("dbo.Experiences", "DoctorId", "dbo.People");
            DropForeignKey("dbo.DailyAvailabilities", "DoctorId", "dbo.People");
            DropForeignKey("dbo.Awards", "DoctorId", "dbo.People");
            DropForeignKey("dbo.Appointments", "PracticeId", "dbo.Practices");
            DropForeignKey("dbo.Appointments", "PatientId", "dbo.People");
            DropForeignKey("dbo.People", "PracticeId", "dbo.Practices");
            DropForeignKey("dbo.PersonFiles", "File_Id", "dbo.Files");
            DropForeignKey("dbo.PersonFiles", "Person_Id", "dbo.People");
            DropForeignKey("dbo.People", "AddressId", "dbo.Addresses");
            DropForeignKey("dbo.Appointments", "DoctorId", "dbo.People");
            DropForeignKey("dbo.Practices", "AddressId", "dbo.Addresses");
            DropIndex("dbo.SpecializationDoctors", new[] { "Doctor_Id" });
            DropIndex("dbo.SpecializationDoctors", new[] { "Specialization_Id" });
            DropIndex("dbo.ServiceDoctors", new[] { "Doctor_Id" });
            DropIndex("dbo.ServiceDoctors", new[] { "Service_Id" });
            DropIndex("dbo.DoctorPractices", new[] { "Practice_Id" });
            DropIndex("dbo.DoctorPractices", new[] { "Doctor_Id" });
            DropIndex("dbo.MembershipDoctors", new[] { "Doctor_Id" });
            DropIndex("dbo.MembershipDoctors", new[] { "Membership_Id" });
            DropIndex("dbo.PersonFiles", new[] { "File_Id" });
            DropIndex("dbo.PersonFiles", new[] { "Person_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Registrations", new[] { "Id" });
            DropIndex("dbo.Qualifications", new[] { "DoctorId" });
            DropIndex("dbo.Experiences", new[] { "DoctorId" });
            DropIndex("dbo.Awards", new[] { "DoctorId" });
            DropIndex("dbo.Appointments", new[] { "PracticeId" });
            DropIndex("dbo.Appointments", new[] { "PatientId" });
            DropIndex("dbo.Appointments", new[] { "DoctorId" });
            DropIndex("dbo.People", new[] { "PracticeId" });
            DropIndex("dbo.People", new[] { "AddressId" });
            DropIndex("dbo.DailyAvailabilities", new[] { "PracticeId" });
            DropIndex("dbo.DailyAvailabilities", new[] { "DoctorId" });
            DropIndex("dbo.Practices", new[] { "AddressId" });
            DropTable("dbo.SpecializationDoctors");
            DropTable("dbo.ServiceDoctors");
            DropTable("dbo.DoctorPractices");
            DropTable("dbo.MembershipDoctors");
            DropTable("dbo.PersonFiles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.DailyAvailabilitySettings");
            DropTable("dbo.CalenderSettings");
            DropTable("dbo.Specializations");
            DropTable("dbo.Services");
            DropTable("dbo.Registrations");
            DropTable("dbo.Qualifications");
            DropTable("dbo.Memberships");
            DropTable("dbo.Experiences");
            DropTable("dbo.Awards");
            DropTable("dbo.Files");
            DropTable("dbo.Appointments");
            DropTable("dbo.People");
            DropTable("dbo.DailyAvailabilities");
            DropTable("dbo.Practices");
            DropTable("dbo.Addresses");
        }
    }
}
