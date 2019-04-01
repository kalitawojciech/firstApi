using System;
using System.Collections.Generic;
using System.Text;

namespace firstApi.Core.Models
{
    public class PersonDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
    }
}
