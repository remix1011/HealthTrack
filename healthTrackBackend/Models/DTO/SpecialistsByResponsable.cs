using healthTrackBackend.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace healthTrackBackend.Models.DTO
{
    public class SpecialistsByResponsable
    {
        public user userResponsible;
        public List<user> userSpecialists;
    }
}