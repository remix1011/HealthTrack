using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace healthTrackBackend.Models.DTO
{
    public class Patient
    {
        public static string SIN_DATOS = "SIN DATOS";

        public int id { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public string fullName { get; set; }
        public string dni { get; set; }
        public System.DateTime birthday { get; set; }
        public int userTypeId { get; set; }
        public string clinicName { get; set; }
        public string enviromentName { get; set; }
        public string diseaseCategoryDescription { get; set; }
        public string treatmentDescription { get; set; }
        public string demeanor { get; set; }
        public int edad { get; set; }
    }
}