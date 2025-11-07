using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagment.Models
{
    public class Appointment
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Doctor ID must be a positive number.")]
        public int did { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Patient ID must be a positive number.")]
        public int pid { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [RegularExpression(@"\d{4}-\d{2}-\d{2}", ErrorMessage = "Date must be in YYYY-MM-DD format.")]
        public string date { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [RegularExpression(@"^([01]\d|2[0-3]):([0-5]\d)$", ErrorMessage = "Time must be in HH:mm format.")]
        public string time { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Disease name must be between 3 and 100 characters.")]
        public string disease { get; set; }
        [Required]
        public string status { get; set; }
    }
}