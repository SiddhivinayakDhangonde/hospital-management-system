using HospitalManagment.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace HospitalManagment.Controllers
{
    public class PatientController : Controller
    {
        // GET: Patient
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PatientDashboard()

        {
            if (Session["name"] == null) {

                return RedirectToAction("PatientLogin", "Home");
            }
            string path = "Data Source=DESKTOP-FH3R9R8\\SQLEXPRESS;Initial Catalog=hospital;Trusted_Connection=true;";
            List<AppointmentSuccess> appointments = new List<AppointmentSuccess>();
            string patientId = Session["id"].ToString();
            using (SqlConnection con = new SqlConnection(path))
            {
                con.Open();
                string query = "SELECT * FROM appointment where patientId=@patientId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@patientId", Session["id"]);
                SqlDataReader br = cmd.ExecuteReader();
                while (br.Read())
                {
                    AppointmentSuccess doc = new AppointmentSuccess
                    {
                        aid = Convert.ToInt32(br["appointmentId"]),
                        date = br["appointdate"].ToString(),
                        time = br["appointime"].ToString(),
                        status = br["appostatus"].ToString()


                    };
                    appointments.Add(doc);
                }
                return View(appointments);
            }
        }
        public ActionResult BookAppointment(int id)
        {
            var model = new Appointment();
            model.did = id; // pre-fill doctor ID
            return View(model);

        }
        [HttpPost]
        public ActionResult BookAppointment(Appointment a1)
        {
            int a = a1.pid;
            int b = a1.did;
            string c = a1.date;
            string d = a1.time;
            string e = a1.disease;
            string f = a1.status;
            string path = "Data Source=DESKTOP-FH3R9R8\\SQLEXPRESS;Initial Catalog= hospital ; Trusted_Connection=true;";
            SqlConnection con = new SqlConnection(path);
            con.Open();
            SqlCommand cmd = new SqlCommand("insert into appointment  (patientId, doctorId, appointdate, appointime, disease,appostatus)" + "VALUES ('" + a + "','" + b + "','" + c + "','" + d + "','" + e + "','" + f + "')", con);
            cmd.ExecuteNonQuery();
            TempData["messege"] = "Appointment Succesful!";
            return RedirectToAction("PatientDashboard", "Patient");
        }
        public ActionResult CancelAppointment(int id)
        {
            string connectionString = "Data Source=DESKTOP-FH3R9R8\\SQLEXPRESS;Initial Catalog=hospital;Trusted_Connection=True;";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM appointment WHERE appointmentId = @aid";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@aid", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            TempData["messege"] = "Appointment cancelled successfully.";
            return RedirectToAction("PatientDashboard", "Patient");
        }




    }
}
