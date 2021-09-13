using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmployeeDataAccess;
namespace WebApiDemo.Controllers
{
    public class EmployeeController : ApiController
    {
        public IEnumerable<Employee> Get()
        {
            using (FirstDBEntities entities = new FirstDBEntities())
            {
                //FirstDBEntities entities = new FirstDBEntities();
                return entities.Employees.ToList();
            }
        }

       // public Employee Get(int id)
       public HttpResponseMessage Get(int id)
        {
            using (FirstDBEntities entities = new FirstDBEntities())
            {
                var emp =entities.Employees.FirstOrDefault(e => e.Id == id);
                if (emp != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, emp);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with id " + id + " not found");
                }

            }
        }

        public HttpResponseMessage Post([FromBody] Employee employee)
        {
            try
            {
                using (FirstDBEntities entities = new FirstDBEntities())
                {
                    //FirstDBEntities entities = new FirstDBEntities();
                    entities.Employees.Add(employee);
                    entities.SaveChanges();
                    var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                    message.Headers.Location = new Uri(Request.RequestUri + employee.Id.ToString());
                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (FirstDBEntities entities = new FirstDBEntities())
                {
                    var entity =entities.Employees.FirstOrDefault(e => e.Id == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with id not found" + id);
                    }
                    else    
                    {
                        entities.Employees.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    
                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,ex); 
            }
           
        }

        public HttpResponseMessage Put(int id,[FromBody]Employee employee)
        {
            try
            {
                using (FirstDBEntities entities = new FirstDBEntities())
                {
                    var entity = entities.Employees.FirstOrDefault(e => e.Id == id);


                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with id not found" + id);
                    }
                    else
                    {
                        entity.Name = employee.Name;
                        entity.Surname = employee.Surname;
                        entity.Role = employee.Role;
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
