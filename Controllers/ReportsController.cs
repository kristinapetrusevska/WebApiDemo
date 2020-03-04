using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using VehicleDataAccess;
using System.Data.Entity;

namespace WebApiDemo.Controllers
{
    [EnableCors("*", "*", "*")]
    public class ReportsController : ApiController
    {
        public IEnumerable<Report> Get(string id)
        {
            using (VehicleDBEntities entities = new VehicleDBEntities())
            {
                var vehRep = entities.VehiclesReports.Include(m => m.Report).Where(veh => veh.vehicleId == id).ToList();
                var reports = vehRep.Select(d => d.Report).ToList();
                    return reports;
            }
            
        }
        
        [HttpPost] //create
        public Report PostReport( string vehId,  [FromBody]Report report)
        {
            using (VehicleDBEntities entities = new VehicleDBEntities())
            {
                Random random = new Random();               
                report.id= random.Next(10000000, 999999999);
                entities.Reports.Add(report);
                var vehrep = new VehiclesReport() {
                 id = random.Next(10000, 99999),
                reportId = report.id,
                    vehicleId = vehId
                };
                entities.VehiclesReports.Add(vehrep);
                entities.SaveChanges();
                return report;
            }

        }
        [HttpPut]
        public Report PutReport([FromBody] Report report)
        {
            using (VehicleDBEntities entities = new VehicleDBEntities())
            {
                var entity = entities.Reports.FirstOrDefault(e => e.id == report.id);

                entity.reportText = report.reportText;                

                entities.SaveChanges();
                return report;
            }
        }
        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (VehicleDBEntities entities = new VehicleDBEntities())
                {
                    var entity = entities.VehiclesReports.FirstOrDefault(e => e.reportId == id);
                    var report = entities.Reports.FirstOrDefault(e => e.id == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            " not found to delete");
                    }
                    else
                    {
                        entities.VehiclesReports.Remove(entity);
                        entities.Reports.Remove(report);
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
