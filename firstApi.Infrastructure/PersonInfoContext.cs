using firstApi.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace firstApi.Infrastructure
{
    public class PersonInfoContext : DbContext
    {
        public PersonInfoContext(DbContextOptions<PersonInfoContext> options) : base(options)
        {
            Database.Migrate();
        }

        public DbSet<Person> People { get; set; }
        public DbSet<Quote> Quotes { get; set; }
    }
}
