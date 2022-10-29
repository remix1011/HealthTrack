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
    public class diseaseCategoryTypesController : ApiController
    {
        private healthtrackdbEntities db = new healthtrackdbEntities();

        // GET: api/diseaseCategoryTypes
        public IQueryable<diseaseCategoryType> GetdiseaseCategoryTypes()
        {
            return db.diseaseCategoryTypes;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}