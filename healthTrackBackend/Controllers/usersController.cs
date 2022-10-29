using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using healthTrackBackend.Models.DTO;
using healthTrackBackend.Models.EF;

namespace healthTrackBackend.Controllers
{
    [RoutePrefix("api/users")]
    public class usersController : ApiController
    {
        private healthtrackdbEntities db = new healthtrackdbEntities();

        // PUT: api/users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putuser(int id, user user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.id)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!userExists(id))
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

        // POST: api/users
        [HttpPost]
        [Route("addUser")]
        [ResponseType(typeof(user))]
        public IHttpActionResult Postuser(user user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.users.Add(user);
            db.SaveChanges();

            return Ok(user);
        }



        // GET: api/users/{id:int}/UserCaringDetails
        [HttpGet]
        [Route("{id:int}/UserCaringDetails")]
        public UserCaringDetails GetUserCaringDetails(int id)
        {
            UserCaringDetails caringDetails = new UserCaringDetails();
            caringDetails.caringEnvironment = db.caringEnvironmentHistories.Where(c => c.userId == id).OrderByDescending(c => c.recordDate).FirstOrDefault();
            caringDetails.diseaseCategory = db.diseaseCategoryHistories.Where(c => c.userId == id).OrderByDescending(c => c.recordDate).FirstOrDefault();
            caringDetails.treatment = db.treatmentHistories.Where(c => c.userId == id).OrderByDescending(c => c.recordDate).FirstOrDefault();
            caringDetails.demeanor = db.demeanorHistories.Where(c => c.userId == id).OrderByDescending(c => c.recordDate).FirstOrDefault();
            caringDetails.recommendations = db.recommendationsHistories.Where(c => c.userId == id).OrderByDescending(c => c.recordDate).FirstOrDefault();

            return caringDetails;
        }
        // GET: api/users/{id:int}/UserMetricsDetailsToday
        [HttpGet]
        [Route("{id:int}/UserMetricsDetailsToday")]
        public UserMetricsDetails GetUserMetricsDetails(int id)
        {
            UserMetricsDetails userMetricsDetails = new UserMetricsDetails();
            //userMetricsDetails.stepsHistory = db.stepsHistories.Where(c => c.userId == id).OrderByDescending(c => c.recordDate).FirstOrDefault();
            //userMetricsDetails.distanceHistory = db.distanceHistories.Where(c => c.userId == id).OrderByDescending(c => c.recordDate).FirstOrDefault();
            //userMetricsDetails.caloriesHistory = db.caloriesHistories.Where(c => c.userId == id).OrderByDescending(c => c.recordDate).FirstOrDefault();
            //userMetricsDetails.heartbeatHistory = db.heartbeatHistories.Where(c => c.userId == id).OrderByDescending(c => c.recordDate).FirstOrDefault();
            //userMetricsDetails.sleepHistory = db.sleepHistories.Where(c => c.userId == id).OrderByDescending(c => c.recordDate).FirstOrDefault();
            //userMetricsDetails.weightHistory = db.weightHistories.Where(c => c.userId == id).OrderByDescending(c => c.recordDate).FirstOrDefault();
            //userMetricsDetails.oximetry = db.oximetries.Where(c => c.userId == id).OrderByDescending(c => c.recordDate).FirstOrDefault();

            DateTime fechaBuscar = DateTime.Now.AddHours(-5).Date;

            userMetricsDetails.stepsHistory = db.stepsHistories.Where(c => c.userId == id && DbFunctions.TruncateTime(c.recordDate).Value == fechaBuscar).FirstOrDefault();
            userMetricsDetails.distanceHistory = db.distanceHistories.Where(c => c.userId == id && DbFunctions.TruncateTime(c.recordDate).Value == fechaBuscar).FirstOrDefault();
            userMetricsDetails.caloriesHistory = db.caloriesHistories.Where(c => c.userId == id && DbFunctions.TruncateTime(c.recordDate).Value == fechaBuscar).FirstOrDefault();
            userMetricsDetails.heartbeatHistory = db.heartbeatHistories.Where(c => c.userId == id && DbFunctions.TruncateTime(c.recordDate).Value == fechaBuscar).FirstOrDefault();
            userMetricsDetails.sleepHistory = db.sleepHistories.Where(c => c.userId == id && DbFunctions.TruncateTime(c.recordDate).Value == fechaBuscar).FirstOrDefault();
            userMetricsDetails.weightHistory = db.weightHistories.Where(c => c.userId == id && DbFunctions.TruncateTime(c.recordDate).Value == fechaBuscar).FirstOrDefault();
            userMetricsDetails.oximetry = db.oximetries.Where(c => c.userId == id && DbFunctions.TruncateTime(c.recordDate).Value == fechaBuscar).FirstOrDefault();

            return userMetricsDetails;
        }
        // GET: api/users/{id:int}/UserMetricsDetailsToday
        [HttpGet]
        [Route("{id:int}/SpecialistAsigned")]
        public IEnumerable<user> GetSpecialistAsigned(int id)
        {
            return db.specialistByResponsables.Where(s => s.idUserResponsable == id).Select(ss => ss.user);
        }



        // GET: api/users/{id:int}/patients
        [HttpGet]
        [Route("{id:int}/patients")]
        public IEnumerable<Patient> GetPatients(int id)
        {
            List<Patient> patients = new List<Patient>();

            db.specialistByResponsables
                .Where(c => c.idUserSpecialist == id && c.active)
                .Select(r => r.user1)
                .ToList()
                .ForEach(u =>
                {
                    var carEnv = db.caringEnvironmentHistories.FirstOrDefault(ce => ce.userId == u.id);
                    var disHis = db.diseaseCategoryHistories.FirstOrDefault(dh => dh.userId == u.id);
                    var treHis = db.treatmentHistories.FirstOrDefault(th => th.userId == u.id);
                    var demHis = db.demeanorHistories.FirstOrDefault(dh => dh.userId == u.id);

                    string uclinicName = carEnv == null ? Patient.SIN_DATOS : carEnv.clinicName;
                    string uenviromentName = carEnv == null ? Patient.SIN_DATOS : carEnv.enviromentName;
                    string udiseaseCategoryDescription = disHis == null ? Patient.SIN_DATOS : disHis.diseaseCategoryType.description;
                    string utreatmentDescription = treHis == null ? Patient.SIN_DATOS : treHis.description;
                    string udemeanor = demHis == null ? Patient.SIN_DATOS : demHis.demeanor;
                    int uedad = Convert.ToInt32((DateTime.Today - u.birthday).Days / 365.25);

                    patients.Add(new Patient()
                    {
                        id = u.id,
                        email = u.email,
                        username = u.username,
                        fullName = u.fullName,
                        dni = u.dni,
                        birthday = u.birthday,
                        userTypeId = u.userTypeId,
                        clinicName = uclinicName,
                        enviromentName = uenviromentName,
                        diseaseCategoryDescription = udiseaseCategoryDescription,
                        treatmentDescription = utreatmentDescription,
                        demeanor = udemeanor,
                        edad = uedad
                    });
                });

            return patients;
        }

        // GET: api/users/{id:int}/responsablesBySpecialist
        [HttpGet]
        [Route("{id:int}/responsablesBySpecialist")]
        public IEnumerable<user> GetResponsablesBySpecialist(int id)
        {
            return db.specialistByResponsables
                .Where(c => c.idUserSpecialist == id && c.active)
                .Select(r => r.user1);
        }

        // GET: api/users/{id:int}/specialistsByResponsable
        [HttpGet]
        [Route("{id:int}/specialistsByResponsable")]
        public IEnumerable<user> GetSpecialistsByResponsable(int id)
        {
            return db.specialistByResponsables
                .Where(c => c.idUserResponsable == id && c.active)
                .Select(r => r.user);
        }

        // GET: api/users/{id:int}/responsablesAvailablesBySpecialist
        [HttpGet]
        [Route("{id:int}/responsablesAvailablesBySpecialist")]
        public IEnumerable<user> GetResponsablesAvailablesBySpecialist(int id)
        {
            var currentPatients = db.specialistByResponsables
                .Where(c => c.idUserSpecialist == id && c.active)
                .Select(r => r.user1);

            return db.users.Where(
                u => u.userTypeId == 2 
                && u.active 
                && !currentPatients.Any(c => c.id == u.id)
            );
        }

        // POST: api/users/addSpecialistByResponsable
        [HttpPost]
        [Route("addSpecialistByResponsable")]
        public IHttpActionResult AddSpecialistByResponsable(specialistByResponsable specialistByResponsable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            specialistByResponsable.active = true;
            db.specialistByResponsables.Add(specialistByResponsable);
            db.SaveChanges();

            return Ok(specialistByResponsable);
        }

        // DELETE: api/users/deleteSpecialistByResponsable
        [HttpDelete]
        [Route("deleteSpecialistByResponsable")]
        public IHttpActionResult DeleteSpecialistByResponsable(specialistByResponsable specialistByResponsable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.specialistByResponsables
                .Where(
                    s => s.idUserResponsable == specialistByResponsable.idUserResponsable 
                    && s.idUserSpecialist == specialistByResponsable.idUserSpecialist
                )
                .ToList()
                .ForEach(r => r.active = false);

            db.SaveChanges();

            return Ok(specialistByResponsable);
        }




        // POST: api/users/addDailyReport
        [HttpPost]
        [Route("addDailyReport")]
        public IHttpActionResult AddDailyReport(DailyReport dailyReport)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (dailyReport.stepsHistory.steps > 0)
                PoststepsHistory(dailyReport.stepsHistory);
            if (dailyReport.distanceHistory.distance > 0)
                PostdistanceHistory(dailyReport.distanceHistory);
            if (dailyReport.caloriesHistory.calories > 0)
                PostcaloriesHistory(dailyReport.caloriesHistory);
            if (dailyReport.heartbeatHistory.averageBpm > 0)
                PostheartbeatHistory(dailyReport.heartbeatHistory);
            if (dailyReport.sleepHistory.averageBpm > -1)
                PostsleepHistory(dailyReport.sleepHistory);
            if (dailyReport.weightHistory.weight > 0)
                PostweightHistory(dailyReport.weightHistory);

            return Ok(dailyReport);
        }
        public void PoststepsHistory(stepsHistory stepsHistory)
        {
            stepsHistory.active = true;
            stepsHistory.recordDate = DateTime.Now.AddHours(-5);
            stepsHistory today = db.stepsHistories.FirstOrDefault(c => c.userId == stepsHistory.userId &&
            DbFunctions.TruncateTime(c.recordDate).Value == DbFunctions.TruncateTime(stepsHistory.recordDate).Value);

            //si ya hay de ese dia se actualiza ese registro
            if (today != null)
            {
                today.steps = stepsHistory.steps;
                today.recordDate = DateTime.Now.AddHours(-5);
                stepsHistory.id = today.id;
            }
            //caso contrario se registra las calorias de ese dia
            else
            {
                db.stepsHistories.Add(stepsHistory);
            }

            db.SaveChanges();
        }
        public void PostdistanceHistory(distanceHistory distanceHistory)
        {
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
        }
        public void PostcaloriesHistory(caloriesHistory caloriesHistory)
        {
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
        }
        public void PostheartbeatHistory(heartbeatHistory heartbeatHistory)
        {
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
        }
        public void PostsleepHistory(sleepHistory sleepHistory)
        {
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
        }
        public void PostweightHistory(weightHistory weightHistory)
        {
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
        }



        // GET: api/users/{id:int}/placesHistory
        [HttpGet]
        [Route("{id:int}/placesHistory")]
        public IEnumerable<placesHistory> GetUserPlacesHistory(int id)
        {
            return db.placesHistories
                .Where(c => c.userId == id && c.active)
                .OrderByDescending(c => c.recordDate);
        }

        // GET: api/users/{id:int}/placesHistory/today
        [HttpGet]
        [Route("{id:int}/placesHistory/today")]
        public IEnumerable<placesHistory> GetUserPlacesHistoryToday(int id)
        {
            return db.placesHistories
                .Where(c => c.userId == id && c.active && DbFunctions.TruncateTime(c.recordDate).Value == DateTime.Today)
                .OrderByDescending(c => c.recordDate);
        }

        // GET: api/users/{id:int}/placesHistory/start/{dayStart:datetime}/end/{dayEnd:datetime}
        [HttpGet]
        [Route("{id:int}/placesHistory/start/{dayStart:datetime}/end/{dayEnd:datetime}")]
        public IEnumerable<placesHistory> GetUserPlacesHistoryFechas(int id, DateTime dayStart, DateTime dayEnd)
        {
            return db.placesHistories
                .Where(c => c.userId == id && c.active && DbFunctions.TruncateTime(c.recordDate).Value >= dayStart && DbFunctions.TruncateTime(c.recordDate).Value <= dayEnd)
                .OrderByDescending(c => c.recordDate);
        }



        // GET: api/users/{id:int}/caloriesHistory
        [HttpGet]
        [Route("{id:int}/caloriesHistory")]
        public IEnumerable<caloriesHistory> GetUserCaloriesHistory(int id)
        {
            return db.caloriesHistories
                .Where(c => c.userId == id && c.active)
                .OrderByDescending(c => c.recordDate);
        }

        // GET: api/users/{id:int}/caloriesHistory/today
        [HttpGet]
        [Route("{id:int}/caloriesHistory/today")]
        public IEnumerable<caloriesHistory> GetUserCaloriesHistoryToday(int id)
        {
            return db.caloriesHistories
                .Where(c => c.userId == id && c.active && DbFunctions.TruncateTime(c.recordDate).Value == DateTime.Today)
                .OrderByDescending(c => c.recordDate);
        }

        // GET: api/users/{id:int}/caloriesHistory/start/{dayStart:datetime}/end/{dayEnd:datetime}
        [HttpGet]
        [Route("{id:int}/caloriesHistory/start/{dayStart:datetime}/end/{dayEnd:datetime}")]
        public IEnumerable<caloriesHistory> GetUserCaloriesHistoryFechas(int id, DateTime dayStart, DateTime dayEnd)
        {
            return db.caloriesHistories
                .Where(c => c.userId == id && c.active && DbFunctions.TruncateTime(c.recordDate).Value >= dayStart && DbFunctions.TruncateTime(c.recordDate).Value <= dayEnd)
                .OrderByDescending(c => c.recordDate);
        }



        // GET: api/users/{id:int}/heartbeatHistory
        [HttpGet]
        [Route("{id:int}/heartbeatHistory")]
        public IEnumerable<heartbeatHistory> GetUserHeartBeatHistory(int id)
        {
            return db.heartbeatHistories
                .Where(c => c.userId == id && c.active)
                .OrderByDescending(c => c.recordDate);
        }

        // GET: api/users/{id:int}/heartbeatHistory/today
        [HttpGet]
        [Route("{id:int}/heartbeatHistory/today")]
        public IEnumerable<heartbeatHistory> GetUserHeartBeatHistoryToday(int id)
        {
            return db.heartbeatHistories
                .Where(c => c.userId == id && c.active && DbFunctions.TruncateTime(c.recordDate).Value == DateTime.Today)
                .OrderByDescending(c => c.recordDate);
        }

        // GET: api/users/{id:int}/heartbeatHistory/start/{dayStart:datetime}/end/{dayEnd:datetime}
        [HttpGet]
        [Route("{id:int}/heartbeatHistory/start/{dayStart:datetime}/end/{dayEnd:datetime}")]
        public IEnumerable<heartbeatHistory> GetUserHeartBeatHistoryFechas(int id, DateTime dayStart, DateTime dayEnd)
        {
            return db.heartbeatHistories
                .Where(c => c.userId == id && c.active && DbFunctions.TruncateTime(c.recordDate).Value >= dayStart && DbFunctions.TruncateTime(c.recordDate).Value <= dayEnd)
                .OrderByDescending(c => c.recordDate);
        }



        // GET: api/users/{id:int}/oximetries
        [HttpGet]
        [Route("{id:int}/oximetries")]
        public IEnumerable<oximetry> GetOximetry(int id)
        {
            return db.oximetries
                .Where(c => c.userId == id && c.active)
                .OrderByDescending(c => c.recordDate);
        }

        // GET: api/users/{id:int}/oximetries/today
        [HttpGet]
        [Route("{id:int}/oximetries/today")]
        public oximetry GetOximetryToday(int id)
        {
            return db.oximetries
                .Where(c => c.userId == id && c.active && DbFunctions.TruncateTime(c.recordDate).Value == DateTime.Today)
                .OrderByDescending(c => c.recordDate).FirstOrDefault();
        }

        // GET: api/users/{id:int}/oximetries/start/{dayStart:datetime}/end/{dayEnd:datetime}
        [HttpGet]
        [Route("{id:int}/oximetries/start/{dayStart:datetime}/end/{dayEnd:datetime}")]
        public IEnumerable<oximetry> GetOximetryFechas(int id, DateTime dayStart, DateTime dayEnd)
        {
            return db.oximetries
                .Where(c => c.userId == id && c.active && DbFunctions.TruncateTime(c.recordDate).Value >= dayStart && DbFunctions.TruncateTime(c.recordDate).Value <= dayEnd)
                .OrderByDescending(c => c.recordDate);
        }



        // GET: api/users/{id:int}/distanceHistory
        [HttpGet]
        [Route("{id:int}/distanceHistory")]
        public IEnumerable<distanceHistory> GetUserDistanceHistory(int id)
        {
            return db.distanceHistories
                .Where(c => c.userId == id && c.active)
                .OrderByDescending(c => c.recordDate);
        }

        // GET: api/users/{id:int}/distanceHistory/today
        [HttpGet]
        [Route("{id:int}/distanceHistory/today")]
        public IEnumerable<distanceHistory> GetUserDistanceHistoryToday(int id)
        {
            return db.distanceHistories
                .Where(c => c.userId == id && c.active && DbFunctions.TruncateTime(c.recordDate).Value == DateTime.Today)
                .OrderByDescending(c => c.recordDate);
        }

        // GET: api/users/{id:int}/distanceHistory/start/{dayStart:datetime}/end/{dayEnd:datetime}
        [HttpGet]
        [Route("{id:int}/distanceHistory/start/{dayStart:datetime}/end/{dayEnd:datetime}")]
        public IEnumerable<distanceHistory> GetUserDistanceHistoryFechas(int id, DateTime dayStart, DateTime dayEnd)
        {
            return db.distanceHistories
                .Where(c => c.userId == id && c.active && DbFunctions.TruncateTime(c.recordDate).Value >= dayStart && DbFunctions.TruncateTime(c.recordDate).Value <= dayEnd)
                .OrderByDescending(c => c.recordDate);
        }



        // GET: api/users/{id:int}/stepsHistory
        [HttpGet]
        [Route("{id:int}/stepsHistory")]
        public IEnumerable<stepsHistory> GetUserStepsHistory(int id)
        {
            return db.stepsHistories
                .Where(c => c.userId == id && c.active)
                .OrderByDescending(c => c.recordDate);
        }

        // GET: api/users/{id:int}/stepsHistory/today
        [HttpGet]
        [Route("{id:int}/stepsHistory/today")]
        public IEnumerable<stepsHistory> GetUserStepsHistoryToday(int id)
        {
            return db.stepsHistories
                .Where(c => c.userId == id && c.active && DbFunctions.TruncateTime(c.recordDate).Value == DateTime.Today)
                .OrderByDescending(c => c.recordDate);
        }

        // GET: api/users/{id:int}/stepsHistory/start/{dayStart:datetime}/end/{dayEnd:datetime}
        [HttpGet]
        [Route("{id:int}/stepsHistory/start/{dayStart:datetime}/end/{dayEnd:datetime}")]
        public IEnumerable<stepsHistory> GetUserStepsHistoryFechas(int id, DateTime dayStart, DateTime dayEnd)
        {
            return db.stepsHistories
                .Where(c => c.userId == id && c.active && DbFunctions.TruncateTime(c.recordDate).Value >= dayStart && DbFunctions.TruncateTime(c.recordDate).Value <= dayEnd)
                .OrderByDescending(c => c.recordDate);
        }



        // GET: api/users/{id:int}/weightHistory
        [HttpGet]
        [Route("{id:int}/weightHistory")]
        public IEnumerable<weightHistory> GetUserWeightHistory(int id)
        {
            return db.weightHistories
                .Where(c => c.userId == id && c.active)
                .OrderByDescending(c => c.recordDate);
        }

        // GET: api/users/{id:int}/weightHistory/today
        [HttpGet]
        [Route("{id:int}/weightHistory/today")]
        public IEnumerable<weightHistory> GetUserWeightHistoryToday(int id)
        {
            return db.weightHistories
                .Where(c => c.userId == id && c.active && DbFunctions.TruncateTime(c.recordDate).Value == DateTime.Today)
                .OrderByDescending(c => c.recordDate);
        }

        // GET: api/users/{id:int}/weightHistory/start/{dayStart:datetime}/end/{dayEnd:datetime}
        [HttpGet]
        [Route("{id:int}/weightHistory/start/{dayStart:datetime}/end/{dayEnd:datetime}")]
        public IEnumerable<weightHistory> GetUserWeightHistoryFechas(int id, DateTime dayStart, DateTime dayEnd)
        {
            return db.weightHistories
                .Where(c => c.userId == id && c.active && DbFunctions.TruncateTime(c.recordDate).Value >= dayStart && DbFunctions.TruncateTime(c.recordDate).Value <= dayEnd)
                .OrderByDescending(c => c.recordDate);
        }



        // GET: api/users/{id:int}/sleepHistory
        [HttpGet]
        [Route("{id:int}/sleepHistory")]
        public IEnumerable<sleepHistory> GetUserSleepHistory(int id)
        {
            return db.sleepHistories
                .Where(c => c.userId == id && c.active)
                .OrderByDescending(c => c.recordDate);
        }

        // GET: api/users/{id:int}/sleepHistory/today
        [HttpGet]
        [Route("{id:int}/sleepHistory/today")]
        public IEnumerable<sleepHistory> GetUserSleepHistoryToday(int id)
        {
            return db.sleepHistories
                .Where(c => c.userId == id && c.active && DbFunctions.TruncateTime(c.recordDate).Value == DateTime.Today)
                .OrderByDescending(c => c.recordDate);
        }

        // GET: api/users/{id:int}/sleepHistory/start/{dayStart:datetime}/end/{dayEnd:datetime}
        [HttpGet]
        [Route("{id:int}/sleepHistory/start/{dayStart:datetime}/end/{dayEnd:datetime}")]
        public IEnumerable<sleepHistory> GetUserSleepHistoryFechas(int id, DateTime dayStart, DateTime dayEnd)
        {
            return db.sleepHistories
                .Where(c => c.userId == id && c.active && DbFunctions.TruncateTime(c.recordDate).Value >= dayStart && DbFunctions.TruncateTime(c.recordDate).Value <= dayEnd)
                .OrderByDescending(c => c.recordDate);
        }



        // GET: api/users/{id:int}/recommendationsHistory
        [HttpGet]
        [Route("{id:int}/recommendationsHistory")]
        public IEnumerable<recommendationsHistory> GetUserRecomendationsHistory(int id)
        {
            return db.recommendationsHistories
                .Where(c => c.userId == id && c.active)
                .OrderByDescending(c => c.recordDate);
        }

        // GET: api/users/{id:int}/recommendationsHistory/today
        [HttpGet]
        [Route("{id:int}/recommendationsHistory/today")]
        public IEnumerable<recommendationsHistory> GetUserRecomendationsHistoryToday(int id)
        {
            return db.recommendationsHistories
                .Where(c => c.userId == id && c.active && DbFunctions.TruncateTime(c.recordDate).Value == DateTime.Today)
                .OrderByDescending(c => c.recordDate);
        }

        // GET: api/users/{id:int}/recommendationsHistory/start/{dayStart:datetime}/end/{dayEnd:datetime}
        [HttpGet]
        [Route("{id:int}/recommendationsHistory/start/{dayStart:datetime}/end/{dayEnd:datetime}")]
        public IEnumerable<recommendationsHistory> GetUserRecomendationsHistoryFechas(int id, DateTime dayStart, DateTime dayEnd)
        {
            return db.recommendationsHistories
                .Where(c => c.userId == id && c.active && DbFunctions.TruncateTime(c.recordDate).Value >= dayStart && DbFunctions.TruncateTime(c.recordDate).Value <= dayEnd)
                .OrderByDescending(c => c.recordDate);
        }



        // GET: api/users/{id:int}/treatmentHistory
        [HttpGet]
        [Route("{id:int}/treatmentHistory")]
        public IEnumerable<treatmentHistory> GetUserTreatmentHistory(int id)
        {
            return db.treatmentHistories
                .Where(c => c.userId == id && c.active)
                .OrderByDescending(c => c.recordDate);
        }

        // GET: api/users/{id:int}/treatmentHistory/today
        [HttpGet]
        [Route("{id:int}/treatmentHistory/today")]
        public IEnumerable<treatmentHistory> GetUserTreatmentHistoryToday(int id)
        {
            return db.treatmentHistories
                .Where(c => c.userId == id && c.active && DbFunctions.TruncateTime(c.recordDate).Value == DateTime.Today)
                .OrderByDescending(c => c.recordDate);
        }

        // GET: api/users/{id:int}/treatmentHistory/start/{dayStart:datetime}/end/{dayEnd:datetime}
        [HttpGet]
        [Route("{id:int}/treatmentHistory/start/{dayStart:datetime}/end/{dayEnd:datetime}")]
        public IEnumerable<treatmentHistory> GetUserTreatmentHistoryFechas(int id, DateTime dayStart, DateTime dayEnd)
        {
            return db.treatmentHistories
                .Where(c => c.userId == id && c.active && DbFunctions.TruncateTime(c.recordDate).Value >= dayStart && DbFunctions.TruncateTime(c.recordDate).Value <= dayEnd)
                .OrderByDescending(c => c.recordDate);
        }



        // GET: api/users/{id:int}/caringEnvironmentHistory
        [HttpGet]
        [Route("{id:int}/caringEnvironmentHistory")]
        public IEnumerable<caringEnvironmentHistory> GetUserCaringEnviromentHistory(int id)
        {
            return db.caringEnvironmentHistories
                .Where(c => c.userId == id && c.active)
                .OrderByDescending(c => c.recordDate);
        }

        // GET: api/users/{id:int}/caringEnvironmentHistory/today
        [HttpGet]
        [Route("{id:int}/caringEnvironmentHistory/today")]
        public IEnumerable<caringEnvironmentHistory> GetUserCaringEnviromentHistoryToday(int id)
        {
            return db.caringEnvironmentHistories
                .Where(c => c.userId == id && c.active && DbFunctions.TruncateTime(c.recordDate).Value == DateTime.Today)
                .OrderByDescending(c => c.recordDate);
        }

        // GET: api/users/{id:int}/caringEnvironmentHistory/start/{dayStart:datetime}/end/{dayEnd:datetime}
        [HttpGet]
        [Route("{id:int}/caringEnvironmentHistory/start/{dayStart:datetime}/end/{dayEnd:datetime}")]
        public IEnumerable<caringEnvironmentHistory> GetUserCaringEnviromentHistoryFechas(int id, DateTime dayStart, DateTime dayEnd)
        {
            return db.caringEnvironmentHistories
                .Where(c => c.userId == id && c.active && DbFunctions.TruncateTime(c.recordDate).Value >= dayStart && DbFunctions.TruncateTime(c.recordDate).Value <= dayEnd)
                .OrderByDescending(c => c.recordDate);
        }



        // GET: api/users/{id:int}/diseaseCategoryHistory
        [HttpGet]
        [Route("{id:int}/diseaseCategoryHistory")]
        public IEnumerable<diseaseCategoryHistory> GetUserDiseaseCategoryHistory(int id)
        {
            return db.diseaseCategoryHistories
                .Where(c => c.userId == id && c.active)
                .OrderByDescending(c => c.recordDate);
        }

        // GET: api/users/{id:int}/diseaseCategoryHistory/today
        [HttpGet]
        [Route("{id:int}/diseaseCategoryHistory/today")]
        public IEnumerable<diseaseCategoryHistory> GetUserDiseaseCategoryHistoryToday(int id)
        {
            return db.diseaseCategoryHistories
                .Where(c => c.userId == id && c.active && DbFunctions.TruncateTime(c.recordDate).Value == DateTime.Today)
                .OrderByDescending(c => c.recordDate);
        }

        // GET: api/users/{id:int}/diseaseCategoryHistory/start/{dayStart:datetime}/end/{dayEnd:datetime}
        [HttpGet]
        [Route("{id:int}/diseaseCategoryHistory/start/{dayStart:datetime}/end/{dayEnd:datetime}")]
        public IEnumerable<diseaseCategoryHistory> GetUserDiseaseCategoryHistoryFechas(int id, DateTime dayStart, DateTime dayEnd)
        {
            return db.diseaseCategoryHistories
                .Where(c => c.userId == id && c.active && DbFunctions.TruncateTime(c.recordDate).Value >= dayStart && DbFunctions.TruncateTime(c.recordDate).Value <= dayEnd)
                .OrderByDescending(c => c.recordDate);
        }



        // GET: api/users/{id:int}/demeanorHistory
        [HttpGet]
        [Route("{id:int}/demeanorHistory")]
        public IEnumerable<demeanorHistory> GetUserDemeanorHistory(int id)
        {
            return db.demeanorHistories
                .Where(c => c.userId == id && c.active)
                .OrderByDescending(c => c.recordDate);
        }

        // GET: api/users/{id:int}/demeanorHistory/today
        [HttpGet]
        [Route("{id:int}/demeanorHistory/today")]
        public IEnumerable<demeanorHistory> GetUserDemeanorHistoryToday(int id)
        {
            return db.demeanorHistories
                .Where(c => c.userId == id && c.active && DbFunctions.TruncateTime(c.recordDate).Value == DateTime.Today)
                .OrderByDescending(c => c.recordDate);
        }

        // GET: api/users/{id:int}/demeanorHistory/start/{dayStart:datetime}/end/{dayEnd:datetime}
        [HttpGet]
        [Route("{id:int}/demeanorHistory/start/{dayStart:datetime}/end/{dayEnd:datetime}")]
        public IEnumerable<demeanorHistory> GetUserDemeanorHistoryFechas(int id, DateTime dayStart, DateTime dayEnd)
        {
            return db.demeanorHistories
                .Where(c => c.userId == id && c.active && DbFunctions.TruncateTime(c.recordDate).Value >= dayStart && DbFunctions.TruncateTime(c.recordDate).Value <= dayEnd)
                .OrderByDescending(c => c.recordDate);
        }

        // POST: api/users/sendResetPasswordEmail
        [HttpPost]
        [Route("sendResetPasswordEmail")]
        public IHttpActionResult PostSendResetPasswordEmail(ResetPasswordDTO rp)
        {
            try
            {
                rp.email = rp.email.ToLower();
                user userFinded = db.users.FirstOrDefault(u => u.email == rp.email && u.dni == rp.dni);

                if (userFinded != null)
                {
                    string charsUpper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    string charsLower = "abcdefghijklmnopqrstuvwxyz";
                    string numbers = "0123456789";
                    string newPassword = "";

                    //creo la nueva contraseña
                    StringBuilder sbnp = new StringBuilder();
                    Random r = new Random();

                    sbnp.Append(numbers[r.Next(0, numbers.Length)]);
                    sbnp.Append(charsUpper[r.Next(0, charsUpper.Length)]);
                    for (int i = 0; i < 7; i++)
                        sbnp.Append(charsLower[r.Next(0, charsLower.Length)]);

                    newPassword = sbnp.ToString();
                    userFinded.password = newPassword;
                    db.SaveChanges();

                    string messageText = "Su nueva contraseña es: " + newPassword;
                    EnviaMail.Enviar(rp.email, messageText);
                    return Ok(new { response = "Ok" });
                }
                else
                {
                    return Conflict();
                }
            }
            catch (Exception e)
            {
                return Ok(new { error = e.StackTrace });
            }
        }

        // PUT: api/users/resetPassword
        [HttpPut]
        [Route("resetPassword")]
        public IHttpActionResult PostResetPassword(NewPasswordDTO np)
        {
            user userFinded = db.users.FirstOrDefault(u => u.id == np.userId);

            if (userFinded != null)
            {
                userFinded.password = np.newPassword;
                db.SaveChanges();
                return Ok(new { response = "Ok" });
            }
            else
            {
                return Conflict();
            }
        }

        // POST: api/users/tokenFCM
        [HttpPost]
        [Route("tokenFCM")]
        public IHttpActionResult PostTokenFcm(TokenFcmDTO tokenFcmDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userFinded = db.users.FirstOrDefault(u => u.id == tokenFcmDTO.userId);
            if (userFinded != null)
            {
                //userFinded.token = tokenFcmDTO.token;
                db.SaveChanges();
                return Ok(userFinded);
            }
            else
            {
                return Conflict();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool userExists(int id)
        {
            return db.users.Count(e => e.id == id) > 0;
        }
    }
}