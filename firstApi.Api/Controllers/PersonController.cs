using firstApi.Core.Entities;
using firstApi.Core.Interfaces;
using firstApi.Core.Models;
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
        private IPersonRepository _personRepository;
        public PersonController(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        [HttpGet()]
        public IActionResult GetPeople()
        {
            var personEntities = _personRepository.GetPeople();
            var result = new List<PersonWithoutQuotesDto>();
            foreach(var personEntity in personEntities)
            {
                result.Add(new PersonWithoutQuotesDto
                {
                    Id = personEntity.Id,
                    FirstName = personEntity.FirstName,
                    LastName = personEntity.LastName,
                    Description = personEntity.Description
                });
            }
            return Ok(result);

        }

        [HttpGet("{id}")]
        public IActionResult GetPerson(int id, bool includeQuotes = false)
        {
            var person = _personRepository.GetPerson(id, includeQuotes);
            if(person == null)
            {
                return NotFound();
            }
            if(includeQuotes)
            {
                var personResult = new PersonDto()
                {
                    Id = person.Id,
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    Description = person.Description
                };
                foreach(var qt in person.Quotes)
                {
                    personResult.Quotes.Add(new QuoteDto()
                    {
                        Id = qt.Id,
                        Description = qt.Description
                    });
                }
                return Ok(personResult);
            }
            var personWithoutQuotesResult = new PersonWithoutQuotesDto()
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Description = person.Description
            };

            return Ok(personWithoutQuotesResult);
            //var person = PeopleDataStore.Current.People.FirstOrDefault(p => p.Id == id);
            //if(person == null)
            //{
            //    return NotFound();
            //}
            //return Ok(person);
        }
    }
}
