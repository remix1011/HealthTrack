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
    public class weightHistoriesController : ApiController
    {
        private healthtrackdbEntities db = new healthtrackdbEntities();

        // PUT: api/weightHistories/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutweightHistory(int id, weightHistory weightHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != weightHistory.id)
            {
                return BadRequest();
            }

            db.Entry(weightHistory).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!weightHistoryExists(id))
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

        // POST: api/weightHistories
        [ResponseType(typeof(weightHistory))]
        public IHttpActionResult PostweightHistory(weightHistory weightHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            weightHistory.active = true;
            weightHistory.recordDate = DateTime.Now.AddHours(-5);
            weightHistory today = db.weightHistories.FirstOrDefault(c => c.userId == weightHistory.userId &&
            DbFunctions.TruncateTime(c.recordDate).Value == DbFunctions.TruncateTime(weightHistory.recordDate).Value);

            //si ya hay de ese dia se actualiza ese registro
            if (today != null)
            {
                today.weight = weightHistory.weight;
                today.recordDate = DateTime.Now.AddHours(-5);
                weightHistory.id = today.id;
            }
            //caso contrario se registra las de ese dia
            else
            {
                db.weightHistories.Add(weightHistory);
            }

            db.SaveChanges();

            return Ok(weightHistory);
        }

        // DELETE: api/weightHistories/5
        [ResponseType(typeof(weightHistory))]
        public IHttpActionResult DeleteweightHistory(int id)
        {
            weightHistory weightHistory = db.weightHistories.Find(id);
            if (weightHistory == null)
            {
                return NotFound();
            }

            db.weightHistories.Remove(weightHistory);
            db.SaveChanges();

            return Ok(weightHistory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool weightHistoryExists(int id)
        {
            return db.weightHistories.Count(e => e.id == id) > 0;
        }
    }
}