using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace firstApi.Core.Models
{
    class QuoteForUpdateDto
    {
        [Required(ErrorMessage = "You should provide a description.")]
        [MaxLength(200)]
        public string Description { get; set; }
    }
}
