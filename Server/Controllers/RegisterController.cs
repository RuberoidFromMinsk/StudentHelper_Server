using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    public class RegisterController : Controller
    {
        NetCoreContext dbContext = new NetCoreContext();
        // POST api/<controller>
        [HttpPost]
        public string Post([FromBody]Student value)
        {
            if(!dbContext.Student.Any(user => user.Login.Equals(value.Login)))
            {
                Student student = new Student();
                student.Id = value.Id;
                student.Login = value.Login;
                student.Password = value.Password;
                student.Name = value.Name;
                student.Mail = value.Mail;
                try
                {
                    dbContext.Add(student);
                    dbContext.SaveChanges();
                    return JsonConvert.SerializeObject("Register successfully - Remote DB");
                }
                catch(Exception e)
                {
                    return JsonConvert.SerializeObject(e.InnerException.Message);
                }
            }
            else
            {
                return JsonConvert.SerializeObject("User already exists - Remote DB");
            }
        }
    }
}
