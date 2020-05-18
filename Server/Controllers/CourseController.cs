using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    public class CourseController : Controller
    {
        NetCoreContext dbContext = new NetCoreContext();

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(string id)
        {
            if (dbContext.Course.Any(x => x.CourseId.Equals(id)))
            {
                try
                {
                    return JsonConvert.SerializeObject(
                        dbContext.Course
                        .Select(u => new { u.CourseId, u.CreatorId, u.Title, u.Description, u.Date })
                        .Where(u => u.CourseId.Equals(id))
                        .First());
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(ex.InnerException.Message);
                }
            }
            else
            {
                return JsonConvert.SerializeObject("Course not found - Remote DB");
            }
        }


        // POST api/<controller>
        [HttpPost]
        public string Post([FromBody]Course value)
        {
            if(!dbContext.Course.Any(course => course.Title.Equals(value.Title)))
            {
                Course course = new Course();
                course.CourseId = value.CourseId;
                course.CreatorId = value.CreatorId;
                course.Title = value.Title;
                course.Description = value.Description;
                course.Date = value.Date;
                try
                {
                    dbContext.Add(course);
                    dbContext.SaveChanges();
                    return JsonConvert.SerializeObject("Insert into Course OK - Remote DB");
                }
                catch(Exception ex)
                {
                    return JsonConvert.SerializeObject(ex.InnerException.Message);
                }
            }
            else
            {
                return JsonConvert.SerializeObject("Course with the same title already exists - Remote DB");
            }
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public string Put(string id, [FromBody]Course value)
        {
            try
            {
                Course course = dbContext.Course.Where(u => u.CourseId.Equals(id)).First();

                course.CourseId = value.CourseId;
                course.CreatorId = value.CreatorId;
                course.Title = value.Title;
                course.Description = value.Description;
                course.Date = value.Date;

                dbContext.Update(course);
                dbContext.SaveChanges();

                return JsonConvert.SerializeObject("Update Course OK - Remote DB");
            }
            catch(Exception ex)
            {
                return JsonConvert.SerializeObject(ex.InnerException.Message);
            }
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public string Delete(string id)
        {
            try
            {
                Course course = dbContext.Course.FirstOrDefault(u => u.CourseId.Equals(id));
                dbContext.Course.Remove(course);
                dbContext.SaveChangesAsync();
                return JsonConvert.SerializeObject("Delete from Course OK - Remote DB");
            }
            catch(Exception ex)
            {
                return JsonConvert.SerializeObject(ex.InnerException.Message);
            }
        }
    }
}
