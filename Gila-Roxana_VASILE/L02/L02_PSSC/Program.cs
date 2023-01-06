using L02_PSSC.Domain;
using static L02_PSSC.Domain.Cart;
using static L02_PSSC.Domain.Quantity;

namespace L02_PSSC
{
    class Program
    {
        private static readonly Random random = new Random();
        static void Main(string[] args)
        {

            var clientMail = new ClientMail(ReadValue("Client Mail: "));

            Address clientAddress = new Address(ReadValue("Address: "));

            var client = new Client(clientMail, clientAddress);

            var listOfProducts = ReadListOfProducts(client).ToArray();

            Guid cartID = Guid.NewGuid();

            EmptyCart emptyCart = new(cartID);
            ICart result = ValidateCart(emptyCart, client);
            result.Match(
                whenEmptyCart: emptyResult => emptyResult,
                whenInvalidatedCart: invalidatedCart => invalidatedCart,
                whenValidatedCart: validatedResult => PayCart(validatedResult),
                whenPayedCart: payedCart => payedCart
                );

            Console.WriteLine(result.ToString());
            foreach (var product in listOfProducts)
            {
                Console.WriteLine(product.productCode.ToString() + " " + product.quantity.GetQ() + " " + product.quantity.ToString());
            }
        }

        private static List<UnvalidatedClientCart> ReadListOfProducts(Client client)
        {
            List<UnvalidatedClientCart> listOfProduct = new();
            int ok = 0;
            do
            {
                ProductCode productCode = new ProductCode(ReadValue("Product Code: "));

                int tipCantitate = Convert.ToInt32(ReadValue("Tip cantitate(0-kg, 1-unitate): "));
                double cantitate = Convert.ToDouble(ReadValue("Cantitate: "));
                IQuantity qty;
                if (tipCantitate == 0)
                {
                    qty = new QKg(cantitate);
                }
                else { qty = new QUnit(cantitate); }

                var product = new Product(productCode, qty);
                listOfProduct.Add(new(productCode, qty));

                ok = Convert.ToInt32(ReadValue("Mai adaugati un produs? 0-Nu; 1-Da"));
            } while (ok != 0);

            return listOfProduct;
        }

        /*
        private static ICart ValidateCart(Cart.EmptyCart emptyCart, Client client) =>
            random.Next(100) > 50 ? 
            throw new Exception("Random error") 
            : new Cart.ValidatedCart(emptyCart.IdCart, client, new List <ValidatedClientCart> ());
        */

        private static ICart ValidateCart(Cart.EmptyCart emptyCart, Client client) =>
           new Cart.ValidatedCart(emptyCart.IdCart, client, new List<ValidatedClientCart>());


        private static ICart PayCart(Cart.ValidatedCart validatedCart) =>
            new PayedCart(validatedCart.IdCart, validatedCart.client, new List<ValidatedClientCart>(), DateTime.Now);

        private static string? ReadValue(string prompt)
        {
            Console.WriteLine(prompt);
            return Console.ReadLine();
        }
    }
}