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
    public class recommendationsHistoriesController : ApiController
    {
        private healthtrackdbEntities db = new healthtrackdbEntities();

        // PUT: api/recommendationsHistories/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutrecommendationsHistory(int id, recommendationsHistory recommendationsHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != recommendationsHistory.id)
            {
                return BadRequest();
            }

            db.Entry(recommendationsHistory).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!recommendationsHistoryExists(id))
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

        // POST: api/recommendationsHistories
        [ResponseType(typeof(recommendationsHistory))]
        public IHttpActionResult PostrecommendationsHistory(recommendationsHistory recommendationsHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            recommendationsHistory.active = true;
            recommendationsHistory.recordDate = DateTime.Now.AddHours(-5);
            db.recommendationsHistories.Add(recommendationsHistory);
            db.SaveChanges();

            return Ok(recommendationsHistory);
        }

        // DELETE: api/recommendationsHistories/5
        [ResponseType(typeof(recommendationsHistory))]
        public IHttpActionResult DeleterecommendationsHistory(int id)
        {
            recommendationsHistory recommendationsHistory = db.recommendationsHistories.Find(id);
            if (recommendationsHistory == null)
            {
                return NotFound();
            }

            db.recommendationsHistories.Remove(recommendationsHistory);
            db.SaveChanges();

            return Ok(recommendationsHistory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool recommendationsHistoryExists(int id)
        {
            return db.recommendationsHistories.Count(e => e.id == id) > 0;
        }
    }
}