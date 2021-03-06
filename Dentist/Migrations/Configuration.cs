using Dentist.Models;
using Dentist.Models.Doctor;
using Dentist.Models.Patient;
using Microsoft.Ajax.Utilities;

namespace Dentist.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Dentist.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;

        }

        protected override void Seed(Dentist.Models.ApplicationDbContext context)
        {
            context.CareServices.AddOrUpdate(x=>x.Name,
                new CareService() { Id = 13, Name = "Wisdom Tooth Extraction" },
                new CareService() { Id = 11, Name = "Smile Makeover" },
                new CareService() { Id = 12, Name = "Painless Root Canal Treatment" },
                new CareService() { Id = 10, Name = "Onlays" },
                new CareService() { Id = 9, Name = "Invisible Braces/Invisalign" },
                new CareService() { Id = 8, Name = "Implantology" },
                new CareService() { Id = 7, Name = "Geriatric Dentistry" },
                new CareService() { Id = 6, Name = "Extractions" },
                new CareService() { Id = 5, Name = "Disimpactions" },
                new CareService() { Id = 4, Name = "Cosmetic Dentistry" },
                new CareService() { Id = 3, Name = "Cast Partial Denture" },
                new CareService() { Id = 2, Name = "Artificial Teeth" },
                new CareService(){ Id = 1, Name = "Acrylic Partial Denture"}
                );

            context.Memberships.AddOrUpdate(x=>x.Name, 
                new Membership(){Id= 1, Name = "Indian Dental Association"},
                new Membership(){Id= 2, Name = "Indian Prosthodontic Society"},
                new Membership(){Id= 3, Name = "Dental Council of India"},
                new Membership(){Id= 4, Name = "Dental Practitioners Forym of India (DPFI)"}
                );

            context.Specializations.AddOrUpdate(x => x.Name,
                new Specialization(){Id = 1, Name = "Dentist"},
                new Specialization(){Id = 2, Name = "Prosthodontist"}
                );

            context.NoteTypes.AddOrUpdate(x => x.Description,
                new NoteType() {Description = "Complaints"},
                new NoteType() {Description = "Observations"},
                new NoteType() {Description = "Investigations"},
                new NoteType() {Description = "Diagnoses"},
                new NoteType() {Description = "Notes"}
                );

            context.Procedures.AddOrUpdate(x => x.Name,
                new Procedure() { Name = "Bleaching" },
                new Procedure() { Name = "Cephalogram" },
                new Procedure() { Name = "Ceramic Braces" },
                new Procedure() { Name = "Ceramic Crown" },
                new Procedure() { Name = "Complex Amalgam Filling" }
                );

            //var practice = newPractice() { Name = "My Dentist" };
            //context.Practices.AddOrUpdate(x => x.Name,
            //    practice,
            //    new Practice() { Name = "My Smile" });

            //var doc1 = new Doctor() { FirstName = "Vishal", LastName = "Goyal"};
            //doc1.Practices.Add(practice);
            //var doc2 = new Doctor() { FirstName = "Prety", LastName = "Goyal" };
            //doc2.Practices.Add(practice);

            //context.Doctors.AddOrUpdate(x => x.FirstName,
            //    doc1,
            //    doc2);


            //context.JobStates.AddOrUpdate(x=>x.Descrition,
            //    new JobState() { Descrition = "Open"},
            //    new JobState() { Descrition = "Done"}
            //    );
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
