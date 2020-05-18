using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        NetCoreContext dbContext = new NetCoreContext();

        // POST api/<controller>
        [HttpPost]
        public string Post([FromBody]Student value)
        {
            if (dbContext.Student.Any(user => user.Login.Equals(value.Login)))
            {
                Student student = dbContext.Student.Where(u => u.Login.Equals(value.Login)).First();
                if (student.Password.Equals(value.Password))
                    return JsonConvert.SerializeObject(student);
                else
                    return JsonConvert.SerializeObject("Wrong password");
            }
            else
            {
                return JsonConvert.SerializeObject("User not exists - Remote DB");
            }
        }
    }
}
