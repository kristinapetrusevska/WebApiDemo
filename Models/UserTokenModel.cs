using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VehicleDataAccess;

namespace WebApiDemo.Models
{
    public class UserTokenModel
    {
        public User user { get; set; }
        public string token { get; set; }
    }
}