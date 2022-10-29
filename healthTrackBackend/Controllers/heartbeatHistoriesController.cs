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
    public class heartbeatHistoriesController : ApiController
    {
        private healthtrackdbEntities db = new healthtrackdbEntities();

        // PUT: api/heartbeatHistories/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutheartbeatHistory(int id, heartbeatHistory heartbeatHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != heartbeatHistory.id)
            {
                return BadRequest();
            }

            db.Entry(heartbeatHistory).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!heartbeatHistoryExists(id))
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

        // POST: api/heartbeatHistories
        [ResponseType(typeof(heartbeatHistory))]
        public IHttpActionResult PostheartbeatHistory(heartbeatHistory heartbeatHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            heartbeatHistory.active = true;
            heartbeatHistory.recordDate = DateTime.Now.AddHours(-5);
            heartbeatHistory today = db.heartbeatHistories.FirstOrDefault(c => c.userId == heartbeatHistory.userId && 
            DbFunctions.TruncateTime(c.recordDate).Value == DbFunctions.TruncateTime(heartbeatHistory.recordDate).Value);

            //si ya hay de ese dia se actualiza ese registro
            if (today != null)
            {
                today.averageBpm = heartbeatHistory.averageBpm;
                today.recordDate = DateTime.Now.AddHours(-5);
                heartbeatHistory.id = today.id;
            }
            //caso contrario se registra las calorias de ese dia
            else
            {
                db.heartbeatHistories.Add(heartbeatHistory);
            }

            db.SaveChanges();

            return Ok(heartbeatHistory);
        }

        // DELETE: api/heartbeatHistories/5
        [ResponseType(typeof(heartbeatHistory))]
        public IHttpActionResult DeleteheartbeatHistory(int id)
        {
            heartbeatHistory heartbeatHistory = db.heartbeatHistories.Find(id);
            if (heartbeatHistory == null)
            {
                return NotFound();
            }

            db.heartbeatHistories.Remove(heartbeatHistory);
            db.SaveChanges();

            return Ok(heartbeatHistory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool heartbeatHistoryExists(int id)
        {
            return db.heartbeatHistories.Count(e => e.id == id) > 0;
        }
    }
}