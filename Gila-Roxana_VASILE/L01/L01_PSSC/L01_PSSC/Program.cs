using System;
using L01_PSSC.Domain;
using CSharp.Choices;
using static L01_PSSC.Domain.Quantity;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
var ContactList = new List<Contact2>();
int option = 0;
do
{
    Console.WriteLine("Alegeti optiunea dvs.");
    Console.WriteLine("0. Iesire");
    Console.WriteLine("1. Adugare conatct si produse");
    Console.WriteLine("2. Afisare lista contacte si produsele lor");
    option = Convert.ToInt32(Console.ReadLine());
    switch(option)
    {
        case 0: break;
        case 1:
            {
                Console.WriteLine("----");
                Console.WriteLine("Nume: ");
                string nume = Console.ReadLine();
                Console.WriteLine("Prenume: ");
                string prenume = Console.ReadLine();
                Console.WriteLine("Nr. telefon: ");
                string nrTel = Console.ReadLine();
                Console.WriteLine("Adresa: ");
                string adresa = Console.ReadLine();
                var contact = new Contact(nume, prenume, nrTel, adresa);

                int ok = 1;
                while(ok!=0)
                {
                    Console.WriteLine("Cod produs: ");
                    string codProd = Console.ReadLine();
                    Console.WriteLine("Tip cantitate(0-kg, 1-unitate): ");
                    int tipCantitate = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Cantitate: ");
                    double cantitate = Convert.ToDouble(Console.ReadLine());
                    IQuantity qty;
                    if (tipCantitate == 0)
                    {
                        qty = new QKg(cantitate);
                    }
                    else { qty = new QUnit(cantitate); }

                    string s = qty.Match(whenQKg: cantitate => "kg",
                                         whenQUnit: cantitate => "unit");
                    Console.WriteLine(s);

                    var listOfProducs = new List<ListOfProducs>();
                    ListOfProducs newItem = new ListOfProducs(codProd, qty);
                    listOfProducs.Add(newItem);

                    ContactList.Add(new(contact, listOfProducs));

                    Console.WriteLine("Mai adaugati un produs? Da - 1, Nu - 0");
                    ok = Convert.ToInt32(Console.ReadLine());
                  
                }

                break;
            }
        case 2:
            {
                foreach (var contacte in ContactList)
                {
                    Console.WriteLine(contacte.Contact.ToString());
                    foreach (var produs in contacte.ListOfProducs)
                    {
                        Console.WriteLine(produs.ProductCode);
                        Console.WriteLine(produs.ProdQunatity); //help here :(
                    }
                }
                break;
            }
        default:
            {
                Console.WriteLine("Dati un nr. de optiune valid!");
                break;
            }
    }

} while (option != 0);