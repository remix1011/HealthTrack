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
    public class demeanorHistoriesController : ApiController
    {
        private healthtrackdbEntities db = new healthtrackdbEntities();

        // PUT: api/demeanorHistories/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutdemeanorHistory(int id, demeanorHistory demeanorHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != demeanorHistory.id)
            {
                return BadRequest();
            }

            db.Entry(demeanorHistory).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!demeanorHistoryExists(id))
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

        // POST: api/demeanorHistories
        [ResponseType(typeof(demeanorHistory))]
        public IHttpActionResult PostdemeanorHistory(demeanorHistory demeanorHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            demeanorHistory.active = true;
            demeanorHistory.recordDate = DateTime.Now.AddHours(-5);
            db.demeanorHistories.Add(demeanorHistory);
            db.SaveChanges();

            return Ok(demeanorHistory);
        }

        // DELETE: api/demeanorHistories/5
        [ResponseType(typeof(demeanorHistory))]
        public IHttpActionResult DeletedemeanorHistory(int id)
        {
            demeanorHistory demeanorHistory = db.demeanorHistories.Find(id);
            if (demeanorHistory == null)
            {
                return NotFound();
            }

            db.demeanorHistories.Remove(demeanorHistory);
            db.SaveChanges();

            return Ok(demeanorHistory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool demeanorHistoryExists(int id)
        {
            return db.demeanorHistories.Count(e => e.id == id) > 0;
        }
    }
}