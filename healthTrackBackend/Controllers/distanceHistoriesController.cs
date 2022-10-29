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
    public class distanceHistoriesController : ApiController
    {
        private healthtrackdbEntities db = new healthtrackdbEntities();

        // PUT: api/distanceHistories/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutdistanceHistory(int id, distanceHistory distanceHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != distanceHistory.id)
            {
                return BadRequest();
            }

            db.Entry(distanceHistory).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!distanceHistoryExists(id))
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

        // POST: api/distanceHistories
        [ResponseType(typeof(distanceHistory))]
        public IHttpActionResult PostdistanceHistory(distanceHistory distanceHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            distanceHistory.active = true;
            distanceHistory.recordDate = DateTime.Now.AddHours(-5);
            distanceHistory today = db.distanceHistories.FirstOrDefault(c => c.userId == distanceHistory.userId &&
            DbFunctions.TruncateTime(c.recordDate).Value == DbFunctions.TruncateTime(distanceHistory.recordDate).Value);

            //si ya hay de ese dia se actualiza ese registro
            if (today != null)
            {
                today.distance = distanceHistory.distance;
                today.recordDate = DateTime.Now.AddHours(-5);
                distanceHistory.id = today.id;
            }
            //caso contrario se registra las calorias de ese dia
            else
            {
                db.distanceHistories.Add(distanceHistory);
            }

            db.SaveChanges();

            return Ok(distanceHistory);
        }

        // DELETE: api/distanceHistories/5
        [ResponseType(typeof(distanceHistory))]
        public IHttpActionResult DeletedistanceHistory(int id)
        {
            distanceHistory distanceHistory = db.distanceHistories.Find(id);
            if (distanceHistory == null)
            {
                return NotFound();
            }

            db.distanceHistories.Remove(distanceHistory);
            db.SaveChanges();

            return Ok(distanceHistory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool distanceHistoryExists(int id)
        {
            return db.distanceHistories.Count(e => e.id == id) > 0;
        }
    }
}