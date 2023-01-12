using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain.Models
{
    //public record CalculatedBillNumber(ClientEmail ClientEmail, Grade ExamGrade, Grade ActivityGrade, Grade FinalGrade, BillAddress BillAddress)
    public record CalculatedBillNumber(ClientEmail ClientEmail, BillAddress BillAddress, BillNumber BillNumber)
    {
        public int BillId { get; set; }
        public bool IsUpdated { get; set; } 
    }
}
