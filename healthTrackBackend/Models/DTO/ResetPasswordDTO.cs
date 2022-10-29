using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace healthTrackBackend.Models.DTO
{
    public class ResetPasswordDTO
    {
        public string email { get; set; }
        public string dni { get; set; }
    }
}