using AutoMapper;
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
            var result = Mapper.Map<IEnumerable<PersonWithoutQuotesDto>>(personEntities);
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
                var personResult = Mapper.Map<PersonDto>(person);
                return Ok(personResult);
            }

            var personWithoutQuotesResult = Mapper.Map<IEnumerable<PersonWithoutQuotesDto>>(person);
            return Ok(personWithoutQuotesResult);
        }
    }
}
