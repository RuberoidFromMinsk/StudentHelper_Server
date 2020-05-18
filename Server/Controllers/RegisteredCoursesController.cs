using System.Collections.Generic;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Data.Common;
using System.IO;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers
{
    [Route("api/[controller]")]
    public class RegisteredCoursesController : Controller
    {
        NetCoreContext dbContext = new NetCoreContext();

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(string id)
        {
            SqlParameter param = new SqlParameter("@id", id);
            List<Course> courses = dbContext.Course.FromSqlRaw("RegisteredCourses @id", param).ToList();
            return JsonConvert.SerializeObject(courses);
        }
    }
}
