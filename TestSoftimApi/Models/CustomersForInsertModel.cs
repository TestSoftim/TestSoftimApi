using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestSoftimApi.Models
{
    public class CustomersForInsertModel
    {
        /// <summary>
        /// customer age
        /// </summary>
        [Required]
        public int Age { get; set; }

        /// <summary>
        /// if the customer was satisfied
        /// </summary>
        [Required]
        public bool WasSatisfied { get; set; }

        /// <summary>
        /// customer's gender
        /// </summary>
        [Required]
        public string Sex { get; set; }
    }
}
