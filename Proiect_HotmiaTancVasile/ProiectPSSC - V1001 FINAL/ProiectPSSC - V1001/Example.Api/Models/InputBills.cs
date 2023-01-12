using Exemple.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Example.Api.Models
{
    public class InputBills
    {
        
        [Required]
        public string ClientEmail { get; set; }
        public string BillAddress { get; set; }

        public string BillNumber { get; set; }
    }
}
