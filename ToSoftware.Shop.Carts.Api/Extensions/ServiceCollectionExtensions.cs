using LM.Domain.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using System.Text;
using ToSoftware.Shop.Carts.Api.Domain;

namespace ToSoftware.Shop.Carts.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddJWTAuthentication(this IServiceCollection services, string key)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        public static void MapCart()
        {
            BsonClassMap.RegisterClassMap<Cart>(cm =>
            {
                cm.AutoMap();
                cm.MapIdProperty(c => c.Id)
                    .SetIdGenerator(StringObjectIdGenerator.Instance)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId));
                cm.MapMember(x => x.CreatedAt).SetDefaultValue(DateTimeHelper.GetCurrentDate());
                cm.MapMember(x => x.LastUpdate).SetDefaultValue(DateTimeHelper.GetCurrentDate());
                cm.MapMember(x => x.Customer);
                cm.MapMember(x => x.Items);
            });
        }
    }
}