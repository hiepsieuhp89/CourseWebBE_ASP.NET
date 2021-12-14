using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;
using CourseWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CourseWebApp.Controllers
{
    public class CourseController : ApiController
    {
        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        public IEnumerable<cours> Get() {

            NSchoolDBContext dbContext = new NSchoolDBContext();

            return dbContext.courses.OrderByDescending(e => e.id).ToList();
        }
        public IEnumerable<cours> Get([FromQuery(Name = "keyword")] string keyword)
        {
            NSchoolDBContext dbContext = new NSchoolDBContext();
            if(keyword != "")
                return dbContext.courses.Where(e => e.name.Contains(keyword) || e.actor.Contains(keyword) || e.code.Contains(keyword)).OrderByDescending(e => e.id).ToList();
            return dbContext.courses.OrderByDescending(e => e.id).ToList();
        }
        public cours Get(int id)
        {
            NSchoolDBContext dbContext = new NSchoolDBContext();

            return dbContext.courses.FirstOrDefault(e => e.id == id);
        }
        public object Post(object course)
        {
            try
            {
                NSchoolDBContext dbContext = new NSchoolDBContext();

                JavaScriptSerializer json_serializer = new JavaScriptSerializer();

                string JSONstring = JsonConvert.SerializeObject(course);

                cours newCourse = JsonConvert.DeserializeObject<cours>(JSONstring);

                dbContext.courses.Add(newCourse);

                return dbContext.SaveChanges() == 1 ? new { status = true } : new { status = false };
            }
            catch(Exception e)
            {   
                return new { status = false };
            }
          
        }
        public object Put(object course)
        {
            try
            {
                NSchoolDBContext dbContext = new NSchoolDBContext();

                JavaScriptSerializer json_serializer = new JavaScriptSerializer();

                string JSONstring = JsonConvert.SerializeObject(course);

                cours courseUpdate = JsonConvert.DeserializeObject<cours>(JSONstring);

                var course_to_update = dbContext.courses.SingleOrDefault(e => e.id == courseUpdate.id);

                course_to_update.name = courseUpdate.name;
                course_to_update.code = courseUpdate.code;
                course_to_update.description = courseUpdate.description;
                course_to_update.actor = courseUpdate.actor;

                return dbContext.SaveChanges() == 1 ? new { status = true } : new { status = false };
            }
            catch (Exception e)
            {
                return new { status = false };
            }
            
        }
        public object Delete(int id)
        {
            try
            {
                NSchoolDBContext dbContext = new NSchoolDBContext();

                //get all course having id
                var course_to_del = dbContext.courses.Where(e => e.id == id);

                foreach(cours course in course_to_del)
                {
                    //remove each entity found
                    dbContext.courses.Remove(course);
                }
                //save
                return dbContext.SaveChanges() == 1 ? new { status = true } : new { status = false };
            }
            catch (Exception e)
            {
                return new { status = false };
            }
        }
        public object Delete(object ids)
        {
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();

                string JSONstring = JsonConvert.SerializeObject(ids);

                int[] idArray = JsonConvert.DeserializeObject<int[]>(JSONstring);

                NSchoolDBContext dbContext = new NSchoolDBContext();

                foreach(int id in idArray)
                {
                    var course_to_del = dbContext.courses.Where(e => e.id == id);

                    foreach (cours course in course_to_del)
                    {
                        //remove each entity found
                        dbContext.courses.Remove(course);
                    }
                }
                //save
                return dbContext.SaveChanges() == 1 ? new { status = true } : new { status = false };
            }
            catch (Exception e)
            {
                return new { status = false };
            }
        }
    }
}
