-- Run this script to setup the database
-- For Dev: Use DurdansClinicDB_Dev
-- For Prod: Use DurdansClinicDB_Prod

CREATE DATABASE DurdansClinicDB_Dev;
GO

USE DurdansClinicDB_Dev;
GO

-- Tables
CREATE TABLE Doctors (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Specialization NVARCHAR(50) NOT NULL,
    ConsultationFee DECIMAL(18, 2) NOT NULL,
    AvailableDays NVARCHAR(100),
    AvailableTime NVARCHAR(100)
);

CREATE TABLE Patients (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    DateOfBirth DATE NOT NULL,
    ContactNumber NVARCHAR(20) NOT NULL
);

CREATE TABLE Appointments (
    Id INT PRIMARY KEY IDENTITY(1,1),
    DoctorId INT FOREIGN KEY REFERENCES Doctors(Id),
    PatientId INT FOREIGN KEY REFERENCES Patients(Id),
    AppointmentDate DATETIME NOT NULL
);
GO

-- Stored Procedures: Doctors
CREATE PROCEDURE sp_InsertDoctor
    @Name NVARCHAR(100),
    @Specialization NVARCHAR(50),
    @ConsultationFee DECIMAL(18, 2),
    @AvailableDays NVARCHAR(100),
    @AvailableTime NVARCHAR(100)
AS
BEGIN
    INSERT INTO Doctors (Name, Specialization, ConsultationFee, AvailableDays, AvailableTime)
    VALUES (@Name, @Specialization, @ConsultationFee, @AvailableDays, @AvailableTime);
    SELECT SCOPE_IDENTITY();
END;
GO

CREATE PROCEDURE sp_UpdateDoctor
    @Id INT,
    @Name NVARCHAR(100),
    @Specialization NVARCHAR(50),
    @ConsultationFee DECIMAL(18, 2),
    @AvailableDays NVARCHAR(100),
    @AvailableTime NVARCHAR(100)
AS
BEGIN
    UPDATE Doctors
    SET Name = @Name,
        Specialization = @Specialization,
        ConsultationFee = @ConsultationFee,
        AvailableDays = @AvailableDays,
        AvailableTime = @AvailableTime
    WHERE Id = @Id;
END;
GO

CREATE PROCEDURE sp_GetAllDoctors
AS
BEGIN
    SELECT * FROM Doctors;
END;
GO

CREATE PROCEDURE sp_GetDoctorById
    @Id INT
AS
BEGIN
    SELECT * FROM Doctors WHERE Id = @Id;
END;
GO

CREATE PROCEDURE sp_GetDoctorsBySpecialization
    @Specialization NVARCHAR(50)
AS
BEGIN
    SELECT * FROM Doctors WHERE Specialization = @Specialization;
END;
GO

-- Stored Procedures: Patients
CREATE PROCEDURE sp_InsertPatient
    @Name NVARCHAR(100),
    @DateOfBirth DATE,
    @ContactNumber NVARCHAR(20)
AS
BEGIN
    INSERT INTO Patients (Name, DateOfBirth, ContactNumber)
    VALUES (@Name, @DateOfBirth, @ContactNumber);
    SELECT SCOPE_IDENTITY();
END;
GO

CREATE PROCEDURE sp_GetAllPatients
AS
BEGIN
    SELECT * FROM Patients;
END;
GO

CREATE PROCEDURE sp_GetPatientById
    @Id INT
AS
BEGIN
    SELECT * FROM Patients WHERE Id = @Id;
END;
GO

-- Stored Procedures: Appointments
CREATE PROCEDURE sp_InsertAppointment
    @DoctorId INT,
    @PatientId INT,
    @AppointmentDate DATETIME
AS
BEGIN
    INSERT INTO Appointments (DoctorId, PatientId, AppointmentDate)
    VALUES (@DoctorId, @PatientId, @AppointmentDate);
    SELECT SCOPE_IDENTITY();
END;
GO

CREATE PROCEDURE sp_GetAllAppointments
AS
BEGIN
    SELECT A.Id, A.DoctorId, A.PatientId, A.AppointmentDate,
           D.Name AS DoctorName, P.Name AS PatientName
    FROM Appointments A
    JOIN Doctors D ON A.DoctorId = D.Id
    JOIN Patients P ON A.PatientId = P.Id;
END;
GO
