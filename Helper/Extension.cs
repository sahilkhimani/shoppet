using shoppetApi.Interfaces;
using shoppetApi.Repository;
using shoppetApi.Services;

namespace shoppetApi.Helper
{
    public static class Extension
    {
        public static void RegisterServices(this IServiceCollection Services) { 
            Services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));

            Services.AddScoped<IUserService, UserService>();
        }

        public static void RegisterRepositories(this IServiceCollection Services)
        {

            Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            Services.AddScoped<IBreedRepository, BreedRepository>();
            Services.AddScoped<IOrderRepository, OrderRepository>();
            Services.AddScoped<IPetRepository, PetRepository>();
            Services.AddScoped<IRoleRepository, RoleRepository>();
            Services.AddScoped<ISpeciesRepository, SpeciesRepository>();
            Services.AddScoped<IUserRepository, UserRepository>();
        }

    }
}
