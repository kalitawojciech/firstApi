using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace firstApi.Core.Entities
{
    public class Quote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Description { get; set; }
        [ForeignKey("PersonId")]
        public Person Person { get; set; }
        public int PersonId { get; set; }
    }
}
