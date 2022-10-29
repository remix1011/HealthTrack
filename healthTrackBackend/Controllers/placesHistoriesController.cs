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
    public class placesHistoriesController : ApiController
    {
        private healthtrackdbEntities db = new healthtrackdbEntities();

        // PUT: api/placesHistories/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutplacesHistory(int id, placesHistory placesHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != placesHistory.id)
            {
                return BadRequest();
            }

            db.Entry(placesHistory).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!placesHistoryExists(id))
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

        // POST: api/placesHistories
        [ResponseType(typeof(placesHistory))]
        public IHttpActionResult PostplacesHistory(placesHistory placesHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            placesHistory.active = true;
            placesHistory.recordDate = DateTime.Now.AddHours(-5);
            db.placesHistories.Add(placesHistory);
            db.SaveChanges();

            return Ok(placesHistory);
        }

        // DELETE: api/placesHistories/5
        [ResponseType(typeof(placesHistory))]
        public IHttpActionResult DeleteplacesHistory(int id)
        {
            placesHistory placesHistory = db.placesHistories.Find(id);
            if (placesHistory == null)
            {
                return NotFound();
            }

            db.placesHistories.Remove(placesHistory);
            db.SaveChanges();

            return Ok(placesHistory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool placesHistoryExists(int id)
        {
            return db.placesHistories.Count(e => e.id == id) > 0;
        }
    }
}