using healthTrackBackend.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace healthTrackBackend.Models.DTO
{
    public class DailyReport
    {
        public stepsHistory stepsHistory { get; set; }
        public distanceHistory distanceHistory { get; set; }
        public caloriesHistory caloriesHistory { get; set; }
        public heartbeatHistory heartbeatHistory { get; set; }
        public sleepHistory sleepHistory { get; set; }
        public weightHistory weightHistory { get; set; } 
    }
}