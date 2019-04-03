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

        [HttpGet("{personId}/quotes/{quoteId}", Name = "GetQuote")]
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

        [HttpPost("{personId}/quote")]
        public IActionResult CreateQuote(int personId, [FromBody] QuoteForCreationDto quote)
        {
            if(quote == null)
            {
                return BadRequest();
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var person = PeopleDataStore.Current.People.FirstOrDefault(p => p.Id == personId);
            if (person == null)
            {
                return NotFound();
            }

            var maxQuoteId = PeopleDataStore.Current.People.SelectMany(q => q.Quotes).Max(q => q.Id);

            var newQuote = new QuoteDto()
            {
                Id = ++maxQuoteId,
                Description = quote.Description
            };

            person.Quotes.Add(newQuote);

            return CreatedAtRoute("GetQuote", new { personId = person.Id, quoteId = newQuote.Id}, newQuote);
        }
    }
}
