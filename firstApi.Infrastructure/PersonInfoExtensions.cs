using firstApi.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace firstApi.Infrastructure
{
    public static class PersonInfoExtensions
    {
        public static void EnsureSeedDataForContext(this PersonInfoContext context)
        {
            if(context.People.Any())
            {
                return;
            }

            var people = new List<Person>()
            {
                new Person()
                {
                    FirstName = "Albert",
                    LastName = "Einstein",
                    Description = "Physicist who developed the theory of relativity.",
                    Quotes = new List<Quote>()
                    {
                        new Quote()
                        {
                            Description = "You cannot blame gravity for falling in love."
                        }
                    }
                },
                new Person()
                {
                    FirstName = "Benjamin",
                    LastName = "Franklin",
                    Description = "One of the Founding Fathers of the United States.",
                    Quotes = new List<Quote>()
                    {
                        new Quote()
                        {
                            Description = "Tell me and I forget. Teach me and I remember. Involve me and I learn."
                        }
                    }
                }
            };
            context.People.AddRange(people);
            context.SaveChanges();
        }
    }
}
