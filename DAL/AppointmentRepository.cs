using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Durdans_WebForms_MVP.Models;

namespace Durdans_WebForms_MVP.DAL
{
    public class AppointmentRepository
    {
        public void InsertAppointment(Appointment appointment)
        {
            SqlHelper.ExecuteScalar("sp_InsertAppointment",
                new SqlParameter("@DoctorId", appointment.DoctorId),
                new SqlParameter("@PatientId", appointment.PatientId),
                new SqlParameter("@AppointmentDate", appointment.AppointmentDate));
        }

        public List<Appointment> GetAllAppointments()
        {
            DataTable dt = SqlHelper.ExecuteReader("sp_GetAllAppointments");
            List<Appointment> appointments = new List<Appointment>();
            foreach (DataRow row in dt.Rows)
            {
                appointments.Add(new Appointment
                {
                    Id = Convert.ToInt32(row["Id"]),
                    DoctorId = Convert.ToInt32(row["DoctorId"]),
                    PatientId = Convert.ToInt32(row["PatientId"]),
                    AppointmentDate = Convert.ToDateTime(row["AppointmentDate"]),
                    // Note: DoctorName and PatientName are joined in SP but not in base Model yet.
                    // If needed, extend Appointment model or use a DTO.
                });
            }
            return appointments;
        }
    }
}
