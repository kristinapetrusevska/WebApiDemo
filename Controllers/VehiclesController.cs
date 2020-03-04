using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
using VehicleDataAccess;

namespace WebApiDemo.Controllers
{
    [EnableCors("*", "*", "*")]
    public class VehiclesController: ApiController
    {
        public IEnumerable<Vehicle> Get()
        {
            using (VehicleDBEntities entities = new VehicleDBEntities())
            {
                return entities.Vehicles.ToList().OrderBy(s=>s.unitName);
            }
        }
        public Vehicle Get(string id)
        {
            using (VehicleDBEntities entities = new VehicleDBEntities())
            {
                return entities.Vehicles.FirstOrDefault(e => e.id == id);
            }
        }
        [HttpGet]
        public IEnumerable<Vehicle> GetSpecific(string searchText)
        {
            using (VehicleDBEntities entities = new VehicleDBEntities())
            {
                var result = entities.Vehicles.Where(d=> d.unitName.Contains(searchText)).ToList();
                return result;
            }
        }

        [HttpPost]
        public Vehicle PostVehicle([FromBody] Vehicle vehicle)
        {
            vehicle.id = generateId();
            using (VehicleDBEntities entities = new VehicleDBEntities())
            {
                entities.Vehicles.Add(vehicle);
                entities.SaveChanges();
                return vehicle;
            }

        }
        private string generateId()
        {
            int length = 7;

            // creating a StringBuilder object()
            StringBuilder str_build = new StringBuilder();
            Random random = new Random();

            char letter;

            for (int i = 0; i < length; i++)
            {
                double flt = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * flt));
                letter = Convert.ToChar(shift + 65);
                str_build.Append(letter);
            }
            return str_build.ToString();
        }
        [HttpPut]
        public void Put(string id, [FromBody]Vehicle vehicle)
        {
            using (VehicleDBEntities entities = new VehicleDBEntities())
            {
                var entity = entities.Vehicles.FirstOrDefault(e => e.id == id);

                entity.unitName = vehicle.unitName;
                entity.carInfo = vehicle.carInfo;
                entity.active = vehicle.active;
                entity.registrationNumber = vehicle.registrationNumber;
                entity.numberOfTours = vehicle.numberOfTours;

                entities.SaveChanges();
            }
        }
        [HttpDelete]
        public HttpResponseMessage Delete(string id)
        {
            try
            {
                using (VehicleDBEntities entities = new VehicleDBEntities())
                {
                    var entity = entities.Vehicles.FirstOrDefault(e => e.id == id);
                    var vehrep = entities.VehiclesReports.Where(d => d.vehicleId == id);                    
                    var reports = vehrep.Select(d => d.Report).ToList();

                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Vehicle with Id = " + id + " not found to delete");
                    }
                    else
                    {
                        foreach (VehiclesReport vp in vehrep) { entities.VehiclesReports.Remove(vp); }
                        foreach (Report report in reports) { entities.Reports.Remove(report); }
                        entities.Vehicles.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    
}
}