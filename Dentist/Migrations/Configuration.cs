using System.Collections.Generic;
using Dentist.Enums;
using Dentist.Models;
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
            ContextKey = "Dentist.Models.ApplicationDbContext";
        }

        protected override void Seed(Dentist.Models.ApplicationDbContext context)
        {
            context.Database.ExecuteSqlCommand("TRUNCATE TABLE Appointments");
            context.Database.ExecuteSqlCommand("TRUNCATE TABLE DailyAvailabilities");
            context.Database.ExecuteSqlCommand("TRUNCATE TABLE PersonPractices");
            context.Database.ExecuteSqlCommand("Delete from Practices");
            context.Database.ExecuteSqlCommand("Delete from people");
            context.Database.ExecuteSqlCommand("Delete from Addresses");
            //context.Database.ExecuteSqlCommand("TRUNCATE TABLE Addresses");
            //context.Database.ExecuteSqlCommand("TRUNCATE TABLE Practices");
            //context.Database.ExecuteSqlCommand("TRUNCATE TABLE People");

            foreach (var app in context.Appointments)
            {
                context.Appointments.Remove(app);
            }

            foreach (var app in context.DailyAvailabilities)
            {
                context.DailyAvailabilities.Remove(app);
            }

            foreach (var app in context.People)
            {
                context.People.Remove(app);
            }

            var practice = new Practice()
            {
                Name = "My Dentist",
                Address = new Address() { AddressLine1 = "22 - A Kamla Nagar" },
                Color = "#c7aa91"
            };

            var practice2 = new Practice()
            {
                Name = "My Practice",
                Address = new Address() { AddressLine1 = "22 - A Kamla Nagar" },
                Color = "#ee9a49"
            };

            var paitient = new Person()
            {
                FirstName = "Gaurav",
                LastName = "Goyal",
                DateOfBirth = new DateTime(1982, 10, 6),
                Address = new Address() { AddressLine1 = "16 Blake Greens" },
                PersonRole = PersonRole.Patient,
                Practices = new List<Practice>() { practice, practice2 },
                Color = "#7ed6cb"
            };

            var doc = new Person()
            {
                FirstName = "Vishal",
                LastName = "Goyal",
                DateOfBirth = new DateTime(1982, 10, 6),
                Address = new Address() { AddressLine1 = "A-22 Kamla Nager" },
                PersonRole = PersonRole.Doctor,
                Practices = new List<Practice>() { practice }
            };

            doc.Appointments = new List<Appointment>()
            {
                new Appointment()
                {
                    Practice = doc.Practices.First(),
                    Doctor = doc,
                    Patient = paitient,
                    StartDateTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 8, 0, 0),
                    EndDateTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 9, 0, 0),
                }
            };


            context.Practices.AddOrUpdate(practice);
            context.Practices.AddOrUpdate(practice2);
            context.People.AddOrUpdate(paitient);
            context.People.AddOrUpdate(doc);
        }
    }
}
