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
    public class caringEnvironmentHistoriesController : ApiController
    {
        private healthtrackdbEntities db = new healthtrackdbEntities();

        // PUT: api/caringEnvironmentHistories/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutcaringEnvironmentHistory(int id, caringEnvironmentHistory caringEnvironmentHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != caringEnvironmentHistory.id)
            {
                return BadRequest();
            }

            db.Entry(caringEnvironmentHistory).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!caringEnvironmentHistoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/caringEnvironmentHistories
        [ResponseType(typeof(caringEnvironmentHistory))]
        public IHttpActionResult PostcaringEnvironmentHistory(caringEnvironmentHistory caringEnvironmentHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            caringEnvironmentHistory.active = true;
            caringEnvironmentHistory.recordDate = DateTime.Now.AddHours(-5);
            db.caringEnvironmentHistories.Add(caringEnvironmentHistory);
            db.SaveChanges();

            return Ok(caringEnvironmentHistory);
        }

        // DELETE: api/caringEnvironmentHistories/5
        [ResponseType(typeof(caringEnvironmentHistory))]
        public IHttpActionResult DeletecaringEnvironmentHistory(int id)
        {
            caringEnvironmentHistory caringEnvironmentHistory = db.caringEnvironmentHistories.Find(id);
            if (caringEnvironmentHistory == null)
            {
                return NotFound();
            }

            db.caringEnvironmentHistories.Remove(caringEnvironmentHistory);
            db.SaveChanges();

            return Ok(caringEnvironmentHistory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool caringEnvironmentHistoryExists(int id)
        {
            return db.caringEnvironmentHistories.Count(e => e.id == id) > 0;
        }
    }
}