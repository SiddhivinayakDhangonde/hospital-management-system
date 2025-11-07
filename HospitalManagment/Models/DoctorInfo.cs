using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagment.Models
{
    public class DoctorInfo
    {
        public int did { get; set; }
        public string name { get; set; }
        public string specialization { get; set; }
        public string phonenumber { get; set; }
        public int fees { get; set; }
        public string time { get; set; }
        public string email { get; set; }
       
    }
}