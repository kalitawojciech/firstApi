using firstApi.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace firstApi.Core.Interfaces
{
    public interface IPersonRepository
    {
        bool PersonExist(int personId);
        IEnumerable<Person> GetPeople();
        Person GetPerson(int personId, bool includeQuotes);
        IEnumerable<Quote> GetQuotesForPerson(int personId);
        Quote GetQuoteForPerson(int personId, int quoteId);
        void AddQuoteForPerson(int personId, Quote quote);
        bool Save();
        void DeleteQuote(Quote quote);
    }
}
