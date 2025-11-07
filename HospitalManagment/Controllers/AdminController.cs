using HospitalManagment.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HospitalManagment.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(AdminLogin a1)

        {
            string name = a1.name;
            string password = a1.password;
            if (name == "Admin@123" && password == "1234")
            {
                Session["name"] = "Admin";
                return RedirectToAction("AdminDashBoard");

            }
            else
            {
                TempData["mes"] = "Invalide credential";
                return RedirectToAction("Index");
            }
        }
        public ActionResult AdminDashBoard()
        {
            string path = "Data Source=DESKTOP-FH3R9R8\\SQLEXPRESS;Initial Catalog=hospital;Trusted_Connection=true;";
            using (SqlConnection con = new SqlConnection(path))
            {
                con.Open();

                SqlCommand sq = new SqlCommand("SELECT COUNT(*) FROM doctorinfo", con);
                int count = (int)sq.ExecuteScalar();
                TempData["doctorcount"] = count;
                SqlCommand sq1= new SqlCommand("SELECT COUNT(*) FROM doctor", con);
                int count1 = (int)sq1.ExecuteScalar();
                TempData["pending"] = count1;
                return View();
            }
        }
       

             public ActionResult DoctorsList()
        {


            string path = "Data Source=DESKTOP-FH3R9R8\\SQLEXPRESS;Initial Catalog=hospital;Trusted_Connection=true;";
            List<DoctorsList> doctors = new List<DoctorsList>();
            using (SqlConnection con = new SqlConnection(path))
            {
                con.Open();

                string query = "SELECT * FROM doctor";
                SqlCommand cmd = new SqlCommand(query, con);

                using (SqlDataReader br = cmd.ExecuteReader())
                {
                    while (br.Read())
                    {
                        DoctorsList doc = new DoctorsList
                        {
                            name = br["dname"].ToString(),
                            specialization = br["deducation"].ToString(),
                            phonenumber = br["dphone"].ToString(),
                            fees = Convert.ToInt32(br["dfees"]),
                            time = br["dtime"].ToString(),
                            password = br["dpassword"].ToString(),
                            email = br["demail"].ToString()
                        };

                        doctors.Add(doc);
                    }
                } // reader is closed here automatically
            }

            return View(doctors);
        }
        
        public ActionResult DoctorApprove(string name)
        {
            string path = "Data Source=DESKTOP-FH3R9R8\\SQLEXPRESS;Initial Catalog=hospital;Trusted_Connection=true;";
            DoctorsList doc = null;

            using (SqlConnection con = new SqlConnection(path))
            {
                con.Open();

                // Step 1: Fetch doctor by name
                using (SqlCommand selectCmd = new SqlCommand("SELECT * FROM doctor WHERE dname = @name", con))
                {
                    selectCmd.Parameters.AddWithValue("@name", name);

                    using (SqlDataReader br = selectCmd.ExecuteReader())
                    {
                        if (br.Read())
                        {
                            doc = new DoctorsList
                            {
                                name = br["dname"].ToString(),
                                specialization = br["deducation"].ToString(),
                                phonenumber = br["dphone"].ToString(),
                                fees = Convert.ToInt32(br["dfees"]),
                                time = br["dtime"].ToString(),
                                password = br["dpassword"].ToString(),
                                email = br["demail"].ToString()
                            };
                        }
                    }
                }

                // Step 2: Insert into doctorinfo if doctor was found
                if (doc != null)
                {

                    using (SqlCommand insertCmd = new SqlCommand(@"
                INSERT INTO doctorinfo 
                (dname, dspecialization, dphone, dfees, davailability, dpassword, demail) 
                VALUES (@dname, @dspecialization, @dphone, @dfees, @davailability, @dpassword, @demail)", con))
                        
                    {
                        insertCmd.Parameters.AddWithValue("@dname", doc.name);
                        insertCmd.Parameters.AddWithValue("@dspecialization", doc.specialization);
                        insertCmd.Parameters.AddWithValue("@dphone", doc.phonenumber);
                        insertCmd.Parameters.AddWithValue("@dfees", doc.fees);
                        insertCmd.Parameters.AddWithValue("@davailability", doc.time);
                        insertCmd.Parameters.AddWithValue("@dpassword", doc.password);
                        insertCmd.Parameters.AddWithValue("@demail", doc.email);

                        insertCmd.ExecuteNonQuery();
                    }
                   

                }
            }

            return RedirectToAction("AdminDashBoard", "Admin");
        }


           
        public ActionResult Cancel(string name)
        {
            string path = "Data Source=DESKTOP-FH3R9R8\\SQLEXPRESS;Initial Catalog=hospital;Trusted_Connection=true;";

            using (SqlConnection con = new SqlConnection(path))
            {
                con.Open();
                string query = "DELETE FROM doctor WHERE dname = @name";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@name", name);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    ViewBag.approve = "Doctor cancelled successfully.";
                }
                else
                {
                    ViewBag.approve = "No doctor found with the given name.";
                }
            }

            return RedirectToAction("AdminDashBoard", "Admin");
        }
        public ActionResult AdminLogout()
        {

            Session.Clear();
            Session.Abandon();

            return RedirectToAction("Index", "Admin");
        }

    }
  }
        
    
