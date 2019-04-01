using firstApi.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace firstApi.Infrastructure
{
    public class PeopleDataStore
    {
        public static PeopleDataStore Current { get; } = new PeopleDataStore();
        public List<PersonDto> People { get; set; }

        public PeopleDataStore()
        {
            People = new List<PersonDto>()
            {
                new PersonDto()
                {
                    Id = 1,
                    FirstName = "Albert",
                    LastName = "Einstein",
                    Description = "Physicist who developed the theory of relativity."
                },
                new PersonDto()
                {
                    Id = 2,
                    FirstName = "Benjamin",
                    LastName = "Franklin",
                    Description = "One of the Founding Fathers of the United States."
                }
            };

        }
    }
}
