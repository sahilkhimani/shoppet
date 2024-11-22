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
using System.Security.Claims;
using System.Text;

namespace shoppetApi.Helper
{
    public static class Extension
    {
        public static void RegisterServices(this IServiceCollection services) { 
            services.AddSingleton<IHttpContextHelper, HttpContextHelper>();
            services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISpeciesService, SpeciesService>();
            services.AddScoped<IBreedService, BreedService>();
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
            var Jwt = configuration.GetSection("Jwt");

            services.AddScoped<JwtTokenService>();
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Jwt["Issuer"],
                        ValidAudience = Jwt["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Jwt["Key"]!))
                    };
                });
        }
    }
}
