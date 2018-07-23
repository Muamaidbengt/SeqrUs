namespace Seqrus.Web.ViewModels
{
    public class SprocketOrderModel
    {
        public int Quantity { get; }
        public decimal Amount { get; }
        public string Address { get; }

        private SprocketOrderModel(int quantity, string address)
        {
            Quantity = quantity;
            Amount = quantity * 42;
            Address = address;
        }

        public static SprocketOrderModel FromCart(SprocketCartModel cart)
        {
            return new SprocketOrderModel(cart.Quantity, cart.Address);
        }
    }
}
