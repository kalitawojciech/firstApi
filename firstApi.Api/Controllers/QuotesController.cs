using AutoMapper;
using firstApi.Core.Entities;
using firstApi.Core.Interfaces;
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
        private IMailService _localMailService;
        private IPersonRepository _personRepository;

        public QuotesController(ILogger<QuotesController> logger, IMailService localMailService, IPersonRepository personRepository)
        {
            _logger = logger;
            _localMailService = localMailService;
            _personRepository = personRepository;
        }

        [HttpGet("{personId}/quotes")]
        public IActionResult GetQuotes(int personId)
        {
            try
            {
                if(!_personRepository.PersonExist(personId))
                {
                    _logger.LogInformation($"Person with id {personId} wasn't found.");
                    return NotFound();
                }

                var quotesForPerson = _personRepository.GetQuotesForPerson(personId);
                var quotesForPersonResult = Mapper.Map<IEnumerable<QuoteDto>>(quotesForPerson);

                return Ok(quotesForPersonResult);
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
            if (!_personRepository.PersonExist(personId))
            {
                _logger.LogInformation($"Person with id {personId} wasn't found.");
                return NotFound();
            }
            var quote = _personRepository.GetQuoteForPerson(personId, quoteId);
            if(quote == null)
            {
                return NotFound();
            }
            var quoteResult = Mapper.Map<QuoteDto>(quote); 
            return Ok(quoteResult);
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

            if(!_personRepository.PersonExist(personId))
            {
                return NotFound();
            }

            var newQuote = Mapper.Map<Quote>(quote);
            _personRepository.AddQuoteForPerson(personId, newQuote);

            if(!_personRepository.Save())
            {
                return StatusCode(500, "Sorry, you did something wrong");
            }

            var createdQuoteToReturn = Mapper.Map<QuoteDto>(newQuote);
            return CreatedAtRoute("GetQuote", new { personId = personId, quoteId = createdQuoteToReturn.Id}, createdQuoteToReturn);
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

            if (!_personRepository.PersonExist(personId))
            {
                return NotFound();
            }

            var quoteEntity = _personRepository.GetQuoteForPerson(personId, quoteId);
            if (quoteEntity == null)
            {
                return BadRequest();
            }

            Mapper.Map(quote, quoteEntity);
            if(!_personRepository.Save())
            {
                return StatusCode(500, "Sorry, something wrong");
            }

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

            if (!_personRepository.PersonExist(personId))
            {
                return NotFound();
            }

            var quoteEntity = _personRepository.GetQuoteForPerson(personId, quoteId);
            if (quoteEntity == null)
            {
                return BadRequest();
            }

            var quoteToPatch = Mapper.Map<QuoteForUpdateDto>(quoteEntity);

            patchDocument.ApplyTo(quoteToPatch, ModelState);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TryValidateModel(quoteToPatch);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mapper.Map(quoteToPatch, quoteEntity);

            if (!_personRepository.Save())
            {
                return StatusCode(500, "Sorry, something wrong");
            }
            return NoContent();
        }

        [HttpDelete("{personId}/quotes/{quoteId}")]
        public IActionResult DeleteQuote(int personId, int quoteId)
        {
            if (!_personRepository.PersonExist(personId))
            {
                return NotFound();
            }

            var quoteEntity = _personRepository.GetQuoteForPerson(personId, quoteId);
            if(quoteEntity == null)
            {
                return NotFound();
            }

            _personRepository.DeleteQuote(quoteEntity);
            if (!_personRepository.Save())
            {
                return StatusCode(500, "Sorry, something wrong");
            }

            _localMailService.Send("Quotes deleted.", $"Quote {quoteEntity.Description} with id {quoteEntity.Id} was deleted.");

            return NoContent();
        }
    }
}
