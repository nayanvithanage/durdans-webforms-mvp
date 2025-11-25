using System.Data.Entity;
using Durdans_WebForms_MVP.Models;

namespace Durdans_WebForms_MVP.Data
{
    public class ClinicDbContext : DbContext
    {
        public ClinicDbContext() : base("name=ClinicDbContext")
        {
        }

        // DbSets for all entities
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<Hospital> Hospitals { get; set; }
        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<DoctorAvailability> DoctorAvailabilities { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Specialization> Specializations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Many-to-Many relationship between Doctor and Hospital
            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.Hospitals)
                .WithMany(h => h.Doctors)
                .Map(m =>
                {
                    m.ToTable("DoctorHospitals");
                    m.MapLeftKey("DoctorId");
                    m.MapRightKey("HospitalId");
                });

            // Configure Doctor-Specialization relationship
            modelBuilder.Entity<Doctor>()
                .HasRequired(d => d.Specialization)
                .WithMany(s => s.Doctors)
                .HasForeignKey(d => d.SpecializationId)
                .WillCascadeOnDelete(false);

            // Configure cascade delete for Appointments
            modelBuilder.Entity<Appointment>()
                .HasRequired(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Appointment>()
                .HasRequired(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Appointment>()
                .HasRequired(a => a.Hospital)
                .WithMany(h => h.Appointments)
                .HasForeignKey(a => a.HospitalId)
                .WillCascadeOnDelete(false);

            // Configure DoctorAvailability relationships
            modelBuilder.Entity<DoctorAvailability>()
                .HasRequired(da => da.Doctor)
                .WithMany(d => d.Availabilities)
                .HasForeignKey(da => da.DoctorId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DoctorAvailability>()
                .HasRequired(da => da.Hospital)
                .WithMany(h => h.DoctorAvailabilities)
                .HasForeignKey(da => da.HospitalId)
                .WillCascadeOnDelete(false);

            // Configure unique constraint on Username
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
        }
    }
}
