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
    public class caloriesHistoriesController : ApiController
    {
        private healthtrackdbEntities db = new healthtrackdbEntities();

        // PUT: api/caloriesHistories/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutcaloriesHistory(int id, caloriesHistory caloriesHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != caloriesHistory.id)
            {
                return BadRequest();
            }

            db.Entry(caloriesHistory).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!caloriesHistoryExists(id))
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

        // POST: api/caloriesHistories
        [ResponseType(typeof(caloriesHistory))]
        public IHttpActionResult PostcaloriesHistory(caloriesHistory caloriesHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            caloriesHistory.active = true;
            caloriesHistory.recordDate = DateTime.Now.AddHours(-5);
            caloriesHistory today = db.caloriesHistories.FirstOrDefault(c => c.userId == caloriesHistory.userId && 
            DbFunctions.TruncateTime(c.recordDate).Value == DbFunctions.TruncateTime(caloriesHistory.recordDate).Value);
            
            //si ya hay de ese dia se actualiza ese registro
            if (today != null)
            {
                today.calories = caloriesHistory.calories;
                today.recordDate = DateTime.Now.AddHours(-5);
                caloriesHistory.id = today.id;
            }
            //caso contrario se registra las de ese dia
            else
            {
                db.caloriesHistories.Add(caloriesHistory);
            }

            db.SaveChanges();

            return Ok(caloriesHistory);
        }

        // DELETE: api/caloriesHistories/5
        [ResponseType(typeof(caloriesHistory))]
        public IHttpActionResult DeletecaloriesHistory(int id)
        {
            caloriesHistory caloriesHistory = db.caloriesHistories.Find(id);
            if (caloriesHistory == null)
            {
                return NotFound();
            }

            db.caloriesHistories.Remove(caloriesHistory);
            db.SaveChanges();

            return Ok(caloriesHistory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool caloriesHistoryExists(int id)
        {
            return db.caloriesHistories.Count(e => e.id == id) > 0;
        }
    }
}