using firstApi.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace firstApi.Api.Controllers
{
    [Route("api/person")]
    public class PersonController : Controller
    {
        [HttpGet()]
        public JsonResult GetPeople()
        {
            return new JsonResult(PeopleDataStore.Current.People);
        }

        [HttpGet("{id}")]
        public JsonResult GetPerson(int id)
        {
            return new JsonResult(
                PeopleDataStore.Current.People.FirstOrDefault(p => p.Id == id)
                );
        }
    }
}
