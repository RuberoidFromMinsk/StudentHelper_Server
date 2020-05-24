using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers
{
    [Route("api/[controller]")]
    public class MemberController : Controller
    {
        NetCoreContext dbContext = new NetCoreContext();

        // GET api/<controller>/5
        [HttpGet("{courseid, id}")]
        public string Get(string courseid, string id)
        {
            if (dbContext.Member.Any(x => x.CourseId.Equals(courseid) && x.MemberId.Equals(id)))
            {
                try
                {
                    return JsonConvert.SerializeObject(
                        dbContext.Member
                        .Select(u => new { u.CourseId, u.MemberId})
                        .Where(u => u.CourseId.Equals(courseid) && u.MemberId.Equals(id))
                        .First());
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(ex.InnerException.Message);
                }
            }
            else
            {
                return JsonConvert.SerializeObject("Member not found - Remote DB");
            }
        }

        // POST api/<controller>
        [HttpPost]
        public string Post([FromBody]Member value)
        {
            Member member = new Member();
            member.CourseId = value.CourseId;
            member.MemberId = value.MemberId;
            try
            {
                dbContext.Add(member);
                dbContext.SaveChanges();
                return JsonConvert.SerializeObject("Insert into Member OK - Remote DB");
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(ex.InnerException.Message);
            }
        }


        // DELETE api/<controller>/5
        [HttpDelete("{courseid}/{id}")]
        public string Delete(string courseid, string id)
        {
            try
            {
                Member member = dbContext.Member.FirstOrDefault(u => u.CourseId.Equals(courseid) && u.MemberId.Equals(id));
                dbContext.Member.Remove(member);
                dbContext.SaveChangesAsync();
                return JsonConvert.SerializeObject("Delete from Member OK - Remote DB");
            }
            catch(Exception ex)
            {
                return JsonConvert.SerializeObject(ex.Message);
            }
        }

        [HttpDelete("{courseId}")]
        public string Delete(string courseId)
        {
            try
            {
                int count = dbContext.Member.Where(x => x.CourseId.Equals(courseId)).Count();
                List<Member> list = new List<Member>();
                for (int i = 0; i < count; i++)
                {
                    Member member = dbContext.Member.Where(u => u.CourseId.Equals(courseId)).OrderBy(u => u.CourseId).Skip(i).FirstOrDefault();
                    list.Add(member);
                }
                dbContext.Member.RemoveRange(list);
                dbContext.SaveChangesAsync();
                //Member member = dbContext.Member.FirstOrDefault(u => u.CourseId.Equals(courseid) && u.MemberId.Equals(id));
                //dbContext.Member.Remove(member);
                //dbContext.SaveChangesAsync();
                return JsonConvert.SerializeObject("Delete from Member OK - Remote DB");
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(ex.Message);
            }
        }



        /*[HttpDelete("{courseId, memberId}")]
        public string Delete(string courseId, string memberId)
        {
            try
            {
                Microsoft.Data.SqlClient.SqlParameter _courseId = new Microsoft.Data.SqlClient.SqlParameter("@cid", courseId);
                Microsoft.Data.SqlClient.SqlParameter _memberId = new Microsoft.Data.SqlClient.SqlParameter("@mid", memberId);
                dbContext.Member.FromSqlRaw("deleteMember @cid, @mid", _courseId, _memberId).ToList();
                return JsonConvert.SerializeObject("Delete from Member OK - Remote DB");
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(ex.InnerException.Message);
            }
        }

        [HttpDelete("{courseId}")]
        public string Delete(string courseId)
        {
            try
            {
                Microsoft.Data.SqlClient.SqlParameter _courseId = new Microsoft.Data.SqlClient.SqlParameter("@cid", courseId);
                dbContext.Member.FromSqlRaw("deleteAllFromCourse @cid", _courseId).ToList();
                return JsonConvert.SerializeObject("Delete all from Member OK - Remote DB");
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(ex.InnerException.Message);
            }
        }*/
    }
}
