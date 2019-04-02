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
        public IActionResult GetPeople()
        {
            return Ok(PeopleDataStore.Current.People);
        }

        [HttpGet("{id}")]
        public IActionResult GetPerson(int id)
        {
            var person = PeopleDataStore.Current.People.FirstOrDefault(p => p.Id == id);
            if(person == null)
            {
                return NotFound();
            }
            return Ok(person);
        }
    }
}
