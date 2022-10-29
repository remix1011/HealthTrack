using healthTrackBackend.Models.DTO;
using healthTrackBackend.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace healthTrackBackend.Controllers
{
    public class loginController : ApiController
    {
        private healthtrackdbEntities db = new healthtrackdbEntities();

        // POST: api/Login
        [ResponseType(typeof(user))]
        public IHttpActionResult Post([FromBody] LoginDTO login)
        {
            user user = db.users.FirstOrDefault(u =>
                u.email == login.email &&
                u.password == login.password
            );

            if (user == null)
            {
                return NotFound();
            }
            if (user.active == false)
            {
                return NotFound();
            }

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
