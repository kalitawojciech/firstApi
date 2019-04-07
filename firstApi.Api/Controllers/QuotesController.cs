using firstApi.Core.Models;
using firstApi.Core.Services;
using firstApi.Infrastructure;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace firstApi.Api.Controllers
{
    [Route("api/person")]
    public class QuotesController : Controller
    {
        private ILogger<QuotesController> _logger;
        private LocalMailService _localMailService;
        public QuotesController(ILogger<QuotesController> logger, LocalMailService localMailService)
        {
            _logger = logger;
            _localMailService = localMailService;
        }

        [HttpGet("{personId}/quotes")]
        public IActionResult GetQuotes(int personId)
        {
            try
            {
                var person = PeopleDataStore.Current.People.FirstOrDefault(p => p.Id == personId);
                if (person == null)
                {
                    _logger.LogInformation($"Person with id {personId} wasn't found.");
                    return NotFound();
                }

                return Ok(person.Quotes);
            }
            catch(Exception exception)
            {
                _logger.LogCritical($"Exception while getting Quotes for person with id {personId}.", exception);
                return StatusCode(500, "Sorry, you did something wrong");
            }


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

        [HttpPost("{personId}/quotes")]
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

        [HttpPut("{personId}/quotes/{quoteId}")]
        public IActionResult UpdateQuote(int personId, int quoteId,
            [FromBody] QuoteForUpdateDto quote)
        {
            if (quote == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var person = PeopleDataStore.Current.People.FirstOrDefault(p => p.Id == personId);
            if (person == null)
            {
                return NotFound();
            }

            var quoteFromStore = person.Quotes.FirstOrDefault(q => q.Id == quoteId);
            if(quoteFromStore == null)
            {
                return NotFound();
            }

            quoteFromStore.Description = quote.Description;

            return NoContent();
        }

        [HttpPatch("{personId}/quotes/{quoteId}")]
        public IActionResult PartiallyUpdateQuote(int personId, int quoteId,
            [FromBody] JsonPatchDocument<QuoteForUpdateDto> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var person = PeopleDataStore.Current.People.FirstOrDefault(p => p.Id == personId);
            if (person == null)
            {
                return NotFound();
            }

            var quoteFromStore = person.Quotes.FirstOrDefault(q => q.Id == quoteId);
            if (quoteFromStore == null)
            {
                return NotFound();
            }

            var quoteToPatch = new QuoteForUpdateDto()
            {
                Description = quoteFromStore.Description
            };

            patchDocument.ApplyTo(quoteToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TryValidateModel(quoteToPatch);

            quoteFromStore.Description = quoteToPatch.Description;

            return NoContent();
        }

        [HttpDelete("{personId}/quotes/{quoteId}")]
        public IActionResult DeleteQuote(int personId, int quoteId)
        {
            var person = PeopleDataStore.Current.People.FirstOrDefault(p => p.Id == personId);
            if (person == null)
            {
                return NotFound();
            }

            var quoteFromStore = person.Quotes.FirstOrDefault(q => q.Id == quoteId);
            if (quoteFromStore == null)
            {
                return NotFound();
            }

            person.Quotes.Remove(quoteFromStore);

            _localMailService.Send("Quotes deleted.", $"Quote {quoteFromStore.Description} with id {quoteFromStore.Id} was deleted.");

            return NoContent();
        }
    }
}
