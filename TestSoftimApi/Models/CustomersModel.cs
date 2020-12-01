using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestSoftimApi.Models
{
    public class CustomersModel : CustomersForInsertModel
    {
        public int Id { get; set; }
        public DateTime VisitDateTime { get; set; }
    }
}
