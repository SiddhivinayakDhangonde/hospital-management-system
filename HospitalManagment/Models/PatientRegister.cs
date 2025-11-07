using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HospitalManagment.Models
{
    public class PatientRegister
    {
        [Required(ErrorMessage ="Please enter your name")]
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage ="Name Should be characters")]
        [StringLength(30,ErrorMessage="greater than 2 lass than 300")]
        public string name { get; set; }
        [Required(ErrorMessage = "Please enter your Age")]
        public int age { get; set; }
        [Required(ErrorMessage = "Please enter your Email")]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$", ErrorMessage = "InValide Format")]
        public string email { get; set; }
        [Required(ErrorMessage = "Please enter your Contact")]
        public string contact { get; set; }
        [Required(ErrorMessage = "Please enter your Password")]
        public string password { get; set; }
    }
}