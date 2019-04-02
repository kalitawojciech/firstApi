using firstApi.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace firstApi.Api.Controllers
{
    [Route("api/person")]
    public class QuotesController : Controller
    {
        [HttpGet("{personId}/quotes")]
        public IActionResult GetQuotes(int personId)
        {
            var person = PeopleDataStore.Current.People.FirstOrDefault(p => p.Id == personId);
            if(person == null)
            {
                return NotFound();
            }

            return Ok(person.Quotes);
        }

        [HttpGet("{personId}/quotes/{quoteId}")]
        public IActionResult GetQuote(int personId, int quoteId)
        {
            var person = PeopleDataStore.Current.People.FirstOrDefault(p => p.Id == personId);
            if(person == null)
            {
                return NotFound();
            }
            var quote = person.Quotes.FirstOrDefault(q => q.Id == quoteId);
            if(quote == null)
            {
                return NotFound();
            }
            return Ok(quote);
        }
    }
}
