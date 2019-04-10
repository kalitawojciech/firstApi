using System;
using System.Collections.Generic;
using System.Text;

namespace firstApi.Core.Entities
{
    public class PersonWithoutQuotesDto
    {

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
    }
}
