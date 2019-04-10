using firstApi.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace firstApi.Core.Interfaces
{
    public interface IPersonRepository
    {
        IEnumerable<Person> GetPeople();
        Person GetPerson(int personId, bool includeQuotes);
        IEnumerable<Quote> GetQuotesForPerson(int personId);
        Quote GetQuoteForPerson(int personId, int quoteId);
    }
}
