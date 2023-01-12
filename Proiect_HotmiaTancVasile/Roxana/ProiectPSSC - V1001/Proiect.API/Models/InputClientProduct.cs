using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ProiectPSSC.Domain.Models;

namespace ProiectPSSC.Api.Models
{
    public class InputClientProduct
    {
        [Required]
        [RegularExpression(ClientEmail.Pattern)]
        public string ClientMail { get; set; }

        [Required]
        [RegularExpression(ProductCode.Pattern)]
        public string ProdCode { get; set; }

        [Required]
        [Range(1,1000)]
        public int Qunatity { get; set; }

    }
}
