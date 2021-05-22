using LM.Responses;
using LM.Responses.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;
using ToSoftware.Shop.Carts.Api.Domain.Repositories.Contracts;
using ToSoftware.Shop.Carts.Api.Domain.Services.Contracts;
using ToSoftware.Shop.Carts.Api.Messages;

namespace ToSoftware.Shop.Carts.Api.Domain.Services
{
    public class CartService : ICartService
    {
        ICartRepository CartRepository { get; }

        IUser User { get; }

        public CartService(ICartRepository cartRepository
            , IUser user)
        {
            CartRepository = cartRepository;
            User = user;
        }

        public async Task<Response<CartResponseMessage>> CreateAsync(CartRequestMessage requestMessage)
        {
            var response = Response<CartResponseMessage>.Create();

            if (!User.HasUser)
                return response.WithBusinessError("Usuário não autorizado.");

            var cart = Cart.Create(requestMessage, User.Identification);

            if (cart.HasError)
                return response.WithMessages(cart.Messages);

            await CartRepository.CreateOrUpdateAsync(cart);

            return response.SetValue(ToResponseMessage(cart));
        }        

        public async Task<Response<CartResponseMessage>> UpdateAsync(Guid code, CartRequestMessage requestMessage)
        {
            var response = Response<CartResponseMessage>.Create();

            if (!User.HasUser)
                return response.WithBusinessError("Usuário não autorizado.");

            var cart = await CartRepository.FindAsync(code);

            if (!cart.HasValue)
                return response.WithBusinessError(nameof(code)
                    , $"Não foi possível encontrar o carrinho com o código '{code}'.");

            if (response.WithMessages(cart.Value.Update(requestMessage, User.Identification).Messages).HasError)
                return response;

            await CartRepository.CreateOrUpdateAsync(cart);

            return response.SetValue(ToResponseMessage(cart));
        }

        public async Task<Response<CartResponseMessage>> FindAsync(Guid code)
        {
            var response = Response<CartResponseMessage>.Create();

            var cart = await CartRepository.FindAsync(code);

            if (!cart.HasValue)
                return response;

            return response.SetValue(ToResponseMessage(cart));
        }

        static CartResponseMessage ToResponseMessage(Cart cart) => new CartResponseMessage
        {
            Code = cart.Code,
            Products = cart.Items.Select(i => new CartResponseMessage.ProductRequestMessage
            {
                Code = i.Code,
                Quantity = i.Quantity
            }).ToList()
        };
    }
}