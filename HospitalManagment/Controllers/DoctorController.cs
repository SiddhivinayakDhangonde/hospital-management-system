using HospitalManagment.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace HospitalManagment.Controllers
{
    public class DoctorController : Controller
    {
        // GET: Doctor
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DoctorRegistration()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DoctorRegistration(DoctorRegister d1)

        {
            string a = d1.name;
            string b = d1.specilization;
            string c = d1.email;
            string d = d1.phone;
            int g = d1.fees;
            string e = d1.availability;
            string f = d1.password;
            String path = "Data Source=DESKTOP-FH3R9R8\\SQLEXPRESS;Initial Catalog=hospital;Trusted_Connection=true;";
            SqlConnection con = new SqlConnection(path);
            con.Open();

            SqlCommand cmd = new SqlCommand("insert into doctor (dname,deducation,dphone,dfees,dtime,dpassword,demail)" + "values('"+a+"','"+b+"','"+d+"','"+g+"','"+e+"','"+f+"','"+c+"')", con);
            cmd.ExecuteNonQuery();
            ViewData["messa"] = "Registration Successful";

            return RedirectToAction("DoctorRegistration");
        }
        public ActionResult DoctorLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DoctorLogin(DLogin d1) {
            string x = d1.demail;
            string y = d1.dpassword;
            String path = "Data Source=DESKTOP-FH3R9R8\\SQLEXPRESS;Initial Catalog=hospital;Trusted_Connection=true;";
            SqlConnection con = new SqlConnection(path);
            con.Open();
            string query = "SELECT * FROM doctorinfo WHERE demail = @x AND dpassword = @y";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@x",x);
            cmd.Parameters.AddWithValue("@y",y);

            SqlDataReader br = cmd.ExecuteReader();

            if (br.Read()) // ✅ Move to the first record
            {
                Session["doctorname"] = br["dname"];
                Session["did"] = br["did"];
                return RedirectToAction("DoctorDashboard");
            }
            else
            {
                ViewBag.Error = "Invalid login credentials.";
                return View("DoctorLogin");
            }
        }
        public ActionResult DoctorDashboard()
        {
            return View();

        }
        public ActionResult PAppointments()
        {
            // Safely retrieve doctorId from session
            if (Session["did"] == null)
            {
                return RedirectToAction("DoctorLogin", "Doctor"); // Or handle appropriately
            }

            int doctorId;
            if (!int.TryParse(Session["did"].ToString(), out doctorId))
            {
                return RedirectToAction("DoctorLogin", "Doctor"); // Or handle appropriately
            }

            string connectionString = "Data Source=DESKTOP-FH3R9R8\\SQLEXPRESS;Initial Catalog=hospital;Trusted_Connection=true;";
            List<PatientDetails> patients = new List<PatientDetails>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
                SELECT 
                    patient.pid,
                    patient.pname,
                    appointment.appointdate AS date,
                    appointment.disease,
                    appointment.appostatus AS status
                FROM 
                    patient 
                JOIN  
                    appointment ON patient.pid = appointment.patientId 
                WHERE 
                    appointment.doctorId = @doctorId";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@doctorId", doctorId);
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PatientDetails patient = new PatientDetails
                            {
                                pid = reader.GetInt32(reader.GetOrdinal("pid")),
                                pname = reader["pname"].ToString(),
                                date = Convert.ToDateTime(reader["date"]).ToString("yyyy-MM-dd"), // Format as needed
                                disease = reader["disease"].ToString(),
                                status = reader["status"].ToString()
                            };
                            patients.Add(patient);
                        }
                    }
                }
            }

            return View(patients);

        }



        public ActionResult DoctorDetails()
        {
            string path = "Data Source=DESKTOP-FH3R9R8\\SQLEXPRESS;Initial Catalog=hospital;Trusted_Connection=true;";
            List<DoctorInfo> doctors = new List<DoctorInfo>();
            using (SqlConnection con = new SqlConnection(path))
            {
                con.Open();
                string query = "SELECT * FROM doctorinfo";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader br = cmd.ExecuteReader();
                while (br.Read())
                {
                    DoctorInfo doc = new DoctorInfo
                    {
                        did = Convert.ToInt32(br["did"]),
                        name = br["dname"].ToString(),
                        specialization = br["dspecialization"].ToString(),
                        phonenumber = br["dphone"].ToString(),
                        fees = Convert.ToInt32(br["dfees"]),
                        time = br["davailability"].ToString(),
                        email = br["demail"].ToString(),

                    };
                    doctors.Add(doc);


                }
                return View(doctors);

            }
        }
        // GET: EditAppointment
        [HttpGet]
        public ActionResult EditAppointment(int id)
        {
            EditAppointment model = null;

            string connectionString = "Data Source=DESKTOP-FH3R9R8\\SQLEXPRESS;Initial Catalog=hospital;Trusted_Connection=true;";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = @"SELECT 
                                    patient.pid, 
                                    patient.pname, 
                                    appointment.appointdate AS date, 
                                    appointment.appointime AS time, 
                                    appointment.disease, 
                                    appointment.appostatus AS status 
                                 FROM 
                                    patient 
                                 JOIN 
                                    appointment ON patient.pid = appointment.patientId 
                                 WHERE 
                                    patient.pid = @id";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    model = new EditAppointment
                    {
                        id = Convert.ToInt32(reader["pid"]),
                        name = reader["pname"].ToString(),
                        date = reader["date"].ToString(),
                        time = reader["time"].ToString(),
                        disease = reader["disease"].ToString(),
                        status = reader["status"].ToString()
                    };
                }
            }

            if (model == null)
            {
                return HttpNotFound("Appointment not found.");
            }

            return View(model);
        }

        // POST: EditAppointment
        [HttpPost]
        public ActionResult EditAppointment(EditAppointment e1)
        {
            string connectionString = "Data Source=DESKTOP-FH3R9R8\\SQLEXPRESS;Initial Catalog=hospital;Trusted_Connection=true;";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = @"UPDATE appointment 
                                 SET appointdate = @date, 
                                     appointime = @time, 
                                     appostatus = @status 
                                 WHERE patientId = @id";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", e1.id);
                cmd.Parameters.AddWithValue("@date", e1.date);
                cmd.Parameters.AddWithValue("@time", e1.time);
                cmd.Parameters.AddWithValue("@status", e1.status);
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("PAppointments", "Doctor");
        }
        public ActionResult DoctorLogout()
        {
            Session.Clear();
            Session.Abandon();

            return RedirectToAction("DoctorLogin", "Doctor");
            
        }

    }
}