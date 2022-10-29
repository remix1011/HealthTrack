using healthTrackBackend.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace healthTrackBackend.Models.DTO
{
    public class ResponsablesBySpecialist
    {
        public user userSpecialist;
        public List<user> userResponsibles;
    }
}