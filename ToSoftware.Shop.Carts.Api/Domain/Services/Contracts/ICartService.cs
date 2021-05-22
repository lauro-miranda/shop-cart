using LM.Responses;
using System;
using System.Threading.Tasks;
using ToSoftware.Shop.Carts.Api.Messages;

namespace ToSoftware.Shop.Carts.Api.Domain.Services.Contracts
{
    public interface ICartService
    {
        Task<Response<CartResponseMessage>> CreateAsync(CartRequestMessage requestMessage);

        Task<Response<CartResponseMessage>> UpdateAsync(Guid code, CartRequestMessage requestMessage);

        Task<Response<CartResponseMessage>> FindAsync(Guid code);
    }
}