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
    public class stepsHistoriesController : ApiController
    {
        private healthtrackdbEntities db = new healthtrackdbEntities();

        // PUT: api/stepsHistories/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutstepsHistory(int id, stepsHistory stepsHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != stepsHistory.id)
            {
                return BadRequest();
            }

            db.Entry(stepsHistory).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!stepsHistoryExists(id))
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

        // POST: api/stepsHistories
        [ResponseType(typeof(stepsHistory))]
        public IHttpActionResult PoststepsHistory(stepsHistory stepsHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            stepsHistory.active = true;
            stepsHistory.recordDate = DateTime.Now.AddHours(-5);
            stepsHistory today = db.stepsHistories.FirstOrDefault(c => c.userId == stepsHistory.userId &&
            DbFunctions.TruncateTime(c.recordDate).Value == DbFunctions.TruncateTime(stepsHistory.recordDate).Value);

            //si ya hay de ese dia se actualiza ese registro
            if (today != null)
            {
                today.steps = stepsHistory.steps;
                today.recordDate = DateTime.Now.AddHours(-5);
                stepsHistory.id = today.id;
            }
            //caso contrario se registra las calorias de ese dia
            else
            {
                db.stepsHistories.Add(stepsHistory);
            }

            db.SaveChanges();

            return Ok(stepsHistory);
        }

        // DELETE: api/stepsHistories/5
        [ResponseType(typeof(stepsHistory))]
        public IHttpActionResult DeletestepsHistory(int id)
        {
            stepsHistory stepsHistory = db.stepsHistories.Find(id);
            if (stepsHistory == null)
            {
                return NotFound();
            }

            db.stepsHistories.Remove(stepsHistory);
            db.SaveChanges();

            return Ok(stepsHistory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool stepsHistoryExists(int id)
        {
            return db.stepsHistories.Count(e => e.id == id) > 0;
        }
    }
}