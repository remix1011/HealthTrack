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
    public class oximetriesController : ApiController
    {
        private healthtrackdbEntities db = new healthtrackdbEntities();

        // PUT: api/oximetries/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putoximetry(int id, oximetry oximetry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != oximetry.id)
            {
                return BadRequest();
            }

            db.Entry(oximetry).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!oximetryExists(id))
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

        // POST: api/oximetries
        [ResponseType(typeof(oximetry))]
        public IHttpActionResult Postoximetry(oximetry oximetry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            oximetry.active = true;
            oximetry.recordDate = DateTime.Now.AddHours(-5);
            oximetry today = db.oximetries.FirstOrDefault(c => c.userId == oximetry.userId &&
            DbFunctions.TruncateTime(c.recordDate).Value == DbFunctions.TruncateTime(oximetry.recordDate).Value);

            //si ya hay de ese dia se actualiza ese registro
            if (today != null)
            {
                today.saturation = oximetry.saturation;
                today.recordDate = DateTime.Now.AddHours(-5);
                oximetry.id = today.id;
            }
            //caso contrario se registra las calorias de ese dia
            else
            {
                db.oximetries.Add(oximetry);
            }

            db.SaveChanges();

            return Ok(oximetry);
        }

        // DELETE: api/oximetries/5
        [ResponseType(typeof(oximetry))]
        public IHttpActionResult Deleteoximetry(int id)
        {
            oximetry oximetry = db.oximetries.Find(id);
            if (oximetry == null)
            {
                return NotFound();
            }

            db.oximetries.Remove(oximetry);
            db.SaveChanges();

            return Ok(oximetry);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool oximetryExists(int id)
        {
            return db.oximetries.Count(e => e.id == id) > 0;
        }
    }
}