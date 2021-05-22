using LM.Responses;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using ToSoftware.Shop.Carts.Api.Domain;
using ToSoftware.Shop.Carts.Api.Domain.Repositories.Contracts;
using ToSoftware.Shop.Carts.Api.Settings;

namespace ToSoftware.Shop.Carts.Api.Data
{
    public class CartRepository : ICartRepository
    {
        protected IMongoCollection<Cart> Collection { get; }

        public CartRepository(IOptions<NoSQLSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);

            Collection = database.GetCollection<Cart>(settings.Value.CartCollectionName);
        }

        public async Task CreateOrUpdateAsync(Cart cart)
        {
            await DeleteAsync(cart);
            await Collection.InsertOneAsync(cart);
        }

        public async Task<Maybe<Cart>> FindAsync(Guid code)
            => await (await Collection
                .FindAsync(a => a.Code.Equals(code)))
                .FirstOrDefaultAsync();

        public async Task DeleteAsync(Cart cart)
            => await Collection.DeleteOneAsync(x => x.Code.Equals(cart.Code));
    }
}