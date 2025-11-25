namespace Durdans_WebForms_MVP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DoctorId = c.Int(nullable: false),
                        PatientId = c.Int(nullable: false),
                        HospitalId = c.Int(nullable: false),
                        AppointmentDate = c.DateTime(nullable: false),
                        AppointmentTime = c.Time(nullable: false, precision: 7),
                        BookingType = c.String(maxLength: 50),
                        BookedBy = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Doctors", t => t.DoctorId)
                .ForeignKey("dbo.Hospitals", t => t.HospitalId)
                .ForeignKey("dbo.Patients", t => t.PatientId)
                .Index(t => t.DoctorId)
                .Index(t => t.PatientId)
                .Index(t => t.HospitalId);
            
            CreateTable(
                "dbo.Doctors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        Specialization = c.String(nullable: false, maxLength: 100),
                        ConsultationFee = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DoctorAvailabilities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DoctorId = c.Int(nullable: false),
                        HospitalId = c.Int(nullable: false),
                        DayOfWeek = c.String(nullable: false, maxLength: 20),
                        StartTime = c.Time(nullable: false, precision: 7),
                        EndTime = c.Time(nullable: false, precision: 7),
                        MaxBookingsPerSlot = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Doctors", t => t.DoctorId)
                .ForeignKey("dbo.Hospitals", t => t.HospitalId)
                .Index(t => t.DoctorId)
                .Index(t => t.HospitalId);
            
            CreateTable(
                "dbo.Hospitals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        Address = c.String(maxLength: 500),
                        ContactNumber = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Patients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        DateOfBirth = c.DateTime(nullable: false),
                        ContactNumber = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 100),
                        PasswordHash = c.String(nullable: false, maxLength: 500),
                        Role = c.String(nullable: false, maxLength: 50),
                        Email = c.String(maxLength: 200),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Username, unique: true);
            
            CreateTable(
                "dbo.DoctorHospitals",
                c => new
                    {
                        DoctorId = c.Int(nullable: false),
                        HospitalId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DoctorId, t.HospitalId })
                .ForeignKey("dbo.Doctors", t => t.DoctorId, cascadeDelete: true)
                .ForeignKey("dbo.Hospitals", t => t.HospitalId, cascadeDelete: true)
                .Index(t => t.DoctorId)
                .Index(t => t.HospitalId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appointments", "PatientId", "dbo.Patients");
            DropForeignKey("dbo.Appointments", "HospitalId", "dbo.Hospitals");
            DropForeignKey("dbo.Appointments", "DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.DoctorHospitals", "HospitalId", "dbo.Hospitals");
            DropForeignKey("dbo.DoctorHospitals", "DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.DoctorAvailabilities", "HospitalId", "dbo.Hospitals");
            DropForeignKey("dbo.DoctorAvailabilities", "DoctorId", "dbo.Doctors");
            DropIndex("dbo.DoctorHospitals", new[] { "HospitalId" });
            DropIndex("dbo.DoctorHospitals", new[] { "DoctorId" });
            DropIndex("dbo.Users", new[] { "Username" });
            DropIndex("dbo.DoctorAvailabilities", new[] { "HospitalId" });
            DropIndex("dbo.DoctorAvailabilities", new[] { "DoctorId" });
            DropIndex("dbo.Appointments", new[] { "HospitalId" });
            DropIndex("dbo.Appointments", new[] { "PatientId" });
            DropIndex("dbo.Appointments", new[] { "DoctorId" });
            DropTable("dbo.DoctorHospitals");
            DropTable("dbo.Users");
            DropTable("dbo.Patients");
            DropTable("dbo.Hospitals");
            DropTable("dbo.DoctorAvailabilities");
            DropTable("dbo.Doctors");
            DropTable("dbo.Appointments");
        }
    }
}
