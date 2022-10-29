using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using healthTrackBackend.Models.EF;

namespace healthTrackBackend.Controllers
{
    public class specialistByResponsablesController : ApiController
    {
        private healthtrackdbEntities db = new healthtrackdbEntities();

        // GET: api/specialistByResponsables
        public IQueryable<specialistByResponsable> GetspecialistByResponsables()
        {
            return db.specialistByResponsables;
        }

        // GET: api/specialistByResponsables/5
        [ResponseType(typeof(specialistByResponsable))]
        public IHttpActionResult GetspecialistByResponsable(int id)
        {
            specialistByResponsable specialistByResponsable = db.specialistByResponsables.Find(id);
            if (specialistByResponsable == null)
            {
                return NotFound();
            }

            return Ok(specialistByResponsable);
        }

        // POST: api/specialistByResponsables
        [ResponseType(typeof(specialistByResponsable))]
        public IHttpActionResult PostspecialistByResponsable(specialistByResponsable specialistByResponsable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.specialistByResponsables.Add(specialistByResponsable);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = specialistByResponsable.id }, specialistByResponsable);
        }

        // DELETE: api/specialistByResponsables/5
        [ResponseType(typeof(specialistByResponsable))]
        public IHttpActionResult DeletespecialistByResponsable(int id)
        {
            specialistByResponsable specialistByResponsable = db.specialistByResponsables.Find(id);
            if (specialistByResponsable == null)
            {
                return NotFound();
            }

            db.specialistByResponsables.Remove(specialistByResponsable);
            db.SaveChanges();

            return Ok(specialistByResponsable);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool specialistByResponsableExists(int id)
        {
            return db.specialistByResponsables.Count(e => e.id == id) > 0;
        }
    }
}