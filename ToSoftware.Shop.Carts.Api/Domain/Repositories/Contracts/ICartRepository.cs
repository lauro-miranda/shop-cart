using LM.Responses;
using System;
using System.Threading.Tasks;

namespace ToSoftware.Shop.Carts.Api.Domain.Repositories.Contracts
{
    public interface ICartRepository
    {
        Task CreateOrUpdateAsync(Cart cart);

        Task<Maybe<Cart>> FindAsync(Guid code);
    }
}