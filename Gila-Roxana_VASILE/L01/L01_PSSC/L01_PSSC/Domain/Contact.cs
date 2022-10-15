using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L01_PSSC.Domain
{
    public record Contact
    {
        private string Name { get; }
        private string FirstName { get; }
        private string PhoneNr { get; }
        private string Address { get; }
        public Contact(string name, string firstname, string phone, string address)
        {
            Name = name;
            FirstName = firstname;
            PhoneNr = phone;
            Address = address;
        }
        public override string ToString()
        {
            return "Nume: " + Name + "Prenume: " + FirstName + "Nr. tel: " + PhoneNr + "Adresa: " + Address;
        }
        public String AString()
        {
            return "Nume: " + Name + "Prenume: " + FirstName + "Nr. tel: " + PhoneNr + "Adresa: " + Address;
        }
    }
}
