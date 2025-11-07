using HospitalManagment.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace HospitalManagment.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult PatientRegistration() {

            return View();
        }
        [HttpPost]
        public ActionResult PatientRegistration(PatientRegister p1)
        {
            if (ModelState.IsValid)
            {
                string name = p1.name;
                string email = p1.email;
                int age = p1.age;
                string contact = p1.contact;
                string password = p1.password;
                string path = "Data Source=DESKTOP-FH3R9R8\\SQLEXPRESS;Initial Catalog= hospital ; Trusted_Connection=true; ";
                SqlConnection con = new SqlConnection(path);
                con.Open();
                SqlCommand cmd = new SqlCommand(
        "INSERT INTO patient (pname, pemail, age, pcontact, ppassword) " +
        "VALUES ('" + name + "','" + email + "','" + age + "','" + contact + "','" + password + "')", con);
                cmd.ExecuteNonQuery();
                if (ModelState.IsValid)
                {
                    // Save to database or perform logic
                    ViewBag.Message = "Doctor registered successfully!";
                    ModelState.Clear(); // Clear form
                    return View();
                }

                ViewBag.Message = "Please correct the errors below.";
               
            }
            return View();
        }
         public ActionResult PatientLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PatientLogin(PatientLog l1)
        {
            string email = l1.email;
            string pass = l1.password;

            string path = "Data Source=DESKTOP-FH3R9R8\\SQLEXPRESS;Initial Catalog=hospital;Trusted_Connection=true;";
            using (SqlConnection con = new SqlConnection(path))
            {
                con.Open();
                string query = "SELECT * FROM patient WHERE pemail = @email AND ppassword = @pass";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@pass", pass);

                SqlDataReader br = cmd.ExecuteReader();

                if (br.Read()) // ✅ Move to the first record
                {
                    Session["name"] = br["pname"];
                    Session["id"] = br["pid"];
                    return RedirectToAction("PatientDashboard","Patient");

                }
                else
                {
                    ViewBag.Error = "Invalid login credentials.";
                    return View("PatientLogin");
                }
            }

        }
        

        
        


        public ActionResult Logout()
        {
            Session.Clear(); 
            Session.Abandon(); 

            return RedirectToAction("PatientLogin", "Home");
        }
        
    }
}