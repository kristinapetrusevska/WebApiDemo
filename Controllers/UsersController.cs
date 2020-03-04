using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
using VehicleDataAccess;
using WebApiDemo.Models;
using System.Data.Entity;

namespace WebApiDemo.Controllers
{
    [EnableCors("*", "*", "*")]
    public class UsersController : ApiController
    {
        public Object Get(string username, string password)
        {
            using (UserDBEntities entities = new UserDBEntities())
            {

                User user = entities.Users.FirstOrDefault(d => d.username == username && d.password == password);
                if (user == null)
                {
                    return "Log in failed. The user doesn't exist.";
                }
                else
                { 
                    var token = generateToken();
                    var model = new UserTokenModel() {
                        
                        user = user,
                        token = token
                    };
                    
                    return model; 


                }
            }
        }
               
        private string generateToken()
        {
            int length = 10;

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
        
        

    }
}