using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ToSoftware.Shop.Carts.Api.Data;
using ToSoftware.Shop.Carts.Api.Domain;
using ToSoftware.Shop.Carts.Api.Domain.Repositories.Contracts;
using ToSoftware.Shop.Carts.Api.Domain.Services;
using ToSoftware.Shop.Carts.Api.Domain.Services.Contracts;
using ToSoftware.Shop.Carts.Api.Extensions;
using ToSoftware.Shop.Carts.Api.Settings;

namespace ToSoftware.Shop.Carts.Api
{
    public class Startup
    {
        IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddScoped<IUser, User>()
                .AddTransient<ICartService, CartService>()
                .AddTransient<ICartRepository, CartRepository>();
            services.Configure<NoSQLSettings>(Configuration.GetSection(nameof(NoSQLSettings)));

            services.AddJWTAuthentication(Configuration.GetSection("JTWSettings:Secret").Value);

            services.AddHttpContextAccessor();

            services.AddCors();

            ServiceCollectionExtensions.MapCart();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials());

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}