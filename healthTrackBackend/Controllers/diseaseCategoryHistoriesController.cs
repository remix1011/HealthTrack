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
    public class diseaseCategoryHistoriesController : ApiController
    {
        private healthtrackdbEntities db = new healthtrackdbEntities();

        // PUT: api/diseaseCategoryHistories/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutdiseaseCategoryHistory(int id, diseaseCategoryHistory diseaseCategoryHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != diseaseCategoryHistory.id)
            {
                return BadRequest();
            }

            db.Entry(diseaseCategoryHistory).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!diseaseCategoryHistoryExists(id))
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

        // POST: api/diseaseCategoryHistories
        [ResponseType(typeof(diseaseCategoryHistory))]
        public IHttpActionResult PostdiseaseCategoryHistory(diseaseCategoryHistory diseaseCategoryHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            diseaseCategoryHistory.active = true;
            diseaseCategoryHistory.recordDate = DateTime.Now.AddHours(-5);
            db.diseaseCategoryHistories.Add(diseaseCategoryHistory);
            db.SaveChanges();

            return Ok(diseaseCategoryHistory);
        }

        // DELETE: api/diseaseCategoryHistories/5
        [ResponseType(typeof(diseaseCategoryHistory))]
        public IHttpActionResult DeletediseaseCategoryHistory(int id)
        {
            diseaseCategoryHistory diseaseCategoryHistory = db.diseaseCategoryHistories.Find(id);
            if (diseaseCategoryHistory == null)
            {
                return NotFound();
            }

            db.diseaseCategoryHistories.Remove(diseaseCategoryHistory);
            db.SaveChanges();

            return Ok(diseaseCategoryHistory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool diseaseCategoryHistoryExists(int id)
        {
            return db.diseaseCategoryHistories.Count(e => e.id == id) > 0;
        }
    }
}