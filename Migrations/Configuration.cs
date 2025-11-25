namespace Durdans_WebForms_MVP.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Durdans_WebForms_MVP.Data.ClinicDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Durdans_WebForms_MVP.Data.ClinicDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            context.Specializations.AddOrUpdate(
                s => s.Name,
                new Models.Specialization { Name = "Cardiology", Description = "Heart and cardiovascular system", IsActive = true },
                new Models.Specialization { Name = "Pediatrics", Description = "Children's health", IsActive = true },
                new Models.Specialization { Name = "Dermatology", Description = "Skin conditions", IsActive = true },
                new Models.Specialization { Name = "Neurology", Description = "Nervous system disorders", IsActive = true },
                new Models.Specialization { Name = "Orthopedics", Description = "Bones and joints", IsActive = true },
                new Models.Specialization { Name = "General Medicine", Description = "General health care", IsActive = true }
            );
        }
    }
}
