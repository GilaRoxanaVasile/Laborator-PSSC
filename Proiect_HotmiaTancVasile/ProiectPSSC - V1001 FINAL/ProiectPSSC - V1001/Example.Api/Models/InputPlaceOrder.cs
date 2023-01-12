using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Exemple.Domain.Models;


namespace Example.Api.Models
{
    public class InputPlaceOrder
    {           
            [Required]
            [RegularExpression(ClientEmail.Pattern)]
            public string clientEmail1 { get; }


            [Required]
            [Range(1, 10000000000)]
            public decimal Price { get; }

            public string Csv { get; }

            public DateTime PublishedDate { get; }

    }
}

