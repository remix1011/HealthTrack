using healthTrackBackend.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace healthTrackBackend.Models.DTO
{
    public class UserCaringDetails
    {
        public caringEnvironmentHistory caringEnvironment { get; set; }
        public diseaseCategoryHistory diseaseCategory { get; set; }
        public treatmentHistory treatment { get; set; }
        public demeanorHistory demeanor { get; set; }
        public recommendationsHistory recommendations { get; set; }
    }
}