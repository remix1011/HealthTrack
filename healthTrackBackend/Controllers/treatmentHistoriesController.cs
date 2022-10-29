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
    public class treatmentHistoriesController : ApiController
    {
        private healthtrackdbEntities db = new healthtrackdbEntities();

        // PUT: api/treatmentHistories/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PuttreatmentHistory(int id, treatmentHistory treatmentHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != treatmentHistory.id)
            {
                return BadRequest();
            }

            db.Entry(treatmentHistory).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!treatmentHistoryExists(id))
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

        // POST: api/treatmentHistories
        [ResponseType(typeof(treatmentHistory))]
        public IHttpActionResult PosttreatmentHistory(treatmentHistory treatmentHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            treatmentHistory.active = true;
            treatmentHistory.recordDate = DateTime.Now.AddHours(-5);
            db.treatmentHistories.Add(treatmentHistory);
            db.SaveChanges();

            return Ok(treatmentHistory);
        }

        // DELETE: api/treatmentHistories/5
        [ResponseType(typeof(treatmentHistory))]
        public IHttpActionResult DeletetreatmentHistory(int id)
        {
            treatmentHistory treatmentHistory = db.treatmentHistories.Find(id);
            if (treatmentHistory == null)
            {
                return NotFound();
            }

            db.treatmentHistories.Remove(treatmentHistory);
            db.SaveChanges();

            return Ok(treatmentHistory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool treatmentHistoryExists(int id)
        {
            return db.treatmentHistories.Count(e => e.id == id) > 0;
        }
    }
}