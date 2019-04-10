using firstApi.Core.Entities;
using firstApi.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace firstApi.Infrastructure
{
    public class PersonRepository : IPersonRepository
    {
        private PersonInfoContext _context;

        public PersonRepository(PersonInfoContext context)
        {
            _context = context;
        }

        public bool PersonExist(int personId)
        {
            return _context.People.Any(p => p.Id == personId);
        }

        public IEnumerable<Person> GetPeople()
        {
            return _context.People.OrderBy(p => p.Id).ToList();
        }

        public Person GetPerson(int personId, bool includeQuotes)
        {
            if (includeQuotes)
            {
                return _context.People.Include(p => p.Quotes).Where(p => p.Id == personId).FirstOrDefault();
            }

            return _context.People.Where(p => p.Id == personId).FirstOrDefault();
        }

        public Quote GetQuoteForPerson(int personId, int quoteId)
        {
            return _context.Quotes.Where(p => p.Id == personId && p.Id == quoteId).FirstOrDefault();
        }

        public IEnumerable<Quote> GetQuotesForPerson(int personId)
        {
            return _context.Quotes.Where(p => p.Id == personId).ToList();
        }
    }
}
