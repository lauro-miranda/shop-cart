using System;

namespace ToSoftware.Shop.Carts.Api.Domain
{
    public class CartItem
    {
        public CartItem(Guid code, int quantity)
        {
            Code = code;
            Quantity = quantity;
        }

        public Guid Code { get; private set; }

        public int Quantity { get; private set; }
    }
}