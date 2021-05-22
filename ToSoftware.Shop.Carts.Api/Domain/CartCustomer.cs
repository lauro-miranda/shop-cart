namespace ToSoftware.Shop.Carts.Api.Domain
{
    public class CartCustomer
    {
        public CartCustomer(string identification)
        {
            Identification = identification;
        }

        public string Identification { get; private set; }
    }
}