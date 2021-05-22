using LM.Domain.Entities;
using LM.Domain.Helpers;
using LM.Responses;
using LM.Responses.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using ToSoftware.Shop.Carts.Api.Messages;

namespace ToSoftware.Shop.Carts.Api.Domain
{
    public class Cart
    {        
        Cart(Guid code, CartCustomer customer)
        {
            Code = code;
            Customer = customer;
            CreatedAt = DateTimeHelper.GetCurrentDate();
            DeletedAt = DateTimeHelper.GetCurrentDate();
        }

        public string Id { get; private set; }

        public Guid Code { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public DateTime LastUpdate { get; private set; }

        public DateTime DeletedAt { get; private set; }

        public CartCustomer Customer { get; private set; }

        public ICollection<CartItem> Items { get; private set; } = new HashSet<CartItem>();

        internal Response Update(CartRequestMessage requestMessage, string identification)
        {
            var response = Response.Create();

            if (!Customer.Identification.Equals(identification))
                return response.WithBusinessError("Não é possível alterar o comprador.");

            Items.Clear();

            Items = GetCartItems(requestMessage, this).ToList();

            return response;
        }

        public static Response<Cart> Create(CartRequestMessage requestMessage, string identification)
        {
            var response = Response<Cart>.Create();

            if (requestMessage == null)
                requestMessage = new CartRequestMessage
                {
                    Products = new List<CartRequestMessage.ProductRequestMessage>()
                };

            if (!requestMessage.Products.Any())
                return response.WithBusinessError("Produtos não informados.");

            if (string.IsNullOrEmpty(identification))
                return response.WithBusinessError("Cliente não informado.");

            var cart = new Cart(Guid.NewGuid(), new CartCustomer(identification));

            cart.Items = GetCartItems(requestMessage, cart).ToList();

            return response.SetValue(cart);
        }

        static IEnumerable<CartItem> GetCartItems(CartRequestMessage requestMessage, Cart cart) 
        {
            foreach (var product in requestMessage.Products)
            {
                yield return new CartItem(product.Code, product.Quantity);
            }
        }

        public static implicit operator Cart(Response<Cart> response) => response.Data.Value;

        public static implicit operator Cart(Maybe<Cart> maybe) => maybe.Value;
    }
}