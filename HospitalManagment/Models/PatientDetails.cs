using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagment.Models
{
    public class PatientDetails
    {
        public int pid { get; set; }
        public string pname { get; set; }

        public string date { get; set; }
        public string disease { get; set; }
        public string status { get; set; }

    }
}