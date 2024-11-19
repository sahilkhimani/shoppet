using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using PetShopApi.Data;
using PetShopApi.Models;
using shoppetApi.Interfaces;
using shoppetApi.MyUnitOfWork;
using shoppetApi.Repository;
using shoppetApi.Services;
using System.Text;

namespace shoppetApi.Helper
{
    public static class Extension
    {
        public static void RegisterServices(this IServiceCollection services) { 
            services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));

            services.AddScoped<IUserService, UserService>();
        }

        public static void RegisterRepositories(this IServiceCollection services)
        {

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<IBreedRepository, BreedRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IPetRepository, PetRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ISpeciesRepository, SpeciesRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }

        public static void RegisterDb(this IServiceCollection services, IConfiguration configuration) {
            services.AddDbContext<ApiDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("dbcs")));

            services.AddIdentity<User, Role>(
                options => { })
                .AddEntityFrameworkStores<ApiDbContext>()
                .AddDefaultTokenProviders();
        }

        public static void RegisterUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void JwtAuth(this IServiceCollection services, IConfiguration configuration)
        {
       services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
     .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
        };
    });
            services.AddScoped<JwtTokenService>();
        }

        public static void AuthPolicy(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
                options.AddPolicy("SellerPolicy", policy => policy.RequireRole("Seller"));
                options.AddPolicy("BuyerPolicy", policy => policy.RequireRole("Buyer"));
            });
        }
    }
}
