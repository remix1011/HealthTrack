using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace healthTrackBackend.Models.DTO
{
    public class TokenFcmDTO
    {
        public int userId { get; set; }
        public string token { get; set; }
    }
}