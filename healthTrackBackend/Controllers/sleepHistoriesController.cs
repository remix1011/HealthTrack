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
    public class sleepHistoriesController : ApiController
    {
        private healthtrackdbEntities db = new healthtrackdbEntities();

        // PUT: api/sleepHistories/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutsleepHistory(int id, sleepHistory sleepHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sleepHistory.id)
            {
                return BadRequest();
            }

            db.Entry(sleepHistory).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!sleepHistoryExists(id))
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

        // POST: api/sleepHistories
        [ResponseType(typeof(sleepHistory))]
        public IHttpActionResult PostsleepHistory(sleepHistory sleepHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            sleepHistory.active = true;
            sleepHistory.recordDate = DateTime.Now.AddHours(-5);
            sleepHistory today = db.sleepHistories.FirstOrDefault(c => c.userId == sleepHistory.userId &&
            DbFunctions.TruncateTime(c.recordDate).Value == DbFunctions.TruncateTime(sleepHistory.recordDate).Value);

            //si ya hay de ese dia se actualiza ese registro
            if (today != null)
            {
                today.averageBpm = sleepHistory.averageBpm;
                today.startDateTime = sleepHistory.startDateTime;
                today.endDateTime = sleepHistory.endDateTime;
                today.recordDate = DateTime.Now.AddHours(-5);
                sleepHistory.id = today.id;
            }
            //caso contrario se registra las de ese dia
            else
            {
                db.sleepHistories.Add(sleepHistory);
            }

            db.SaveChanges();

            return Ok(sleepHistory);
        }

        // DELETE: api/sleepHistories/5
        [ResponseType(typeof(sleepHistory))]
        public IHttpActionResult DeletesleepHistory(int id)
        {
            sleepHistory sleepHistory = db.sleepHistories.Find(id);
            if (sleepHistory == null)
            {
                return NotFound();
            }

            db.sleepHistories.Remove(sleepHistory);
            db.SaveChanges();

            return Ok(sleepHistory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool sleepHistoryExists(int id)
        {
            return db.sleepHistories.Count(e => e.id == id) > 0;
        }
    }
}