using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HospitalManagment.Models
{
    public class DoctorRegister
    {
        [Required(ErrorMessage = "Please enter your name")]
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Name Should be characters")]
        public string name { get; set; }
        [Required(ErrorMessage = "Please enter your specilization")]
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Name Should be characters")]
        public string specilization { get; set; }
        [Required(ErrorMessage = "Please enter your Email")]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$", ErrorMessage ="InValide Format")]
        public  string email { get; set; }
        [Required(ErrorMessage = "Please enter your contact number")]
        [RegularExpression("^[0-9]{10}$", ErrorMessage = "InValide Format")]

        public string phone { get; set; }
        [Required(ErrorMessage = "Please enter your fees")]
        public int fees { get; set; }
        [Required(ErrorMessage = "Please enter your available time")]
        public  string availability { get; set; }
        [Required(ErrorMessage ="Password is required")]
        [RegularExpression("^[A-Za-z0-9]{6}$", ErrorMessage ="Invalide format")]
        public string password { get; set; }

    }
}