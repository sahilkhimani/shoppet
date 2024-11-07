using PetShopApi.Data;
using shoppetApi.Interfaces;

namespace shoppetApi.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApiDbContext _context;
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();
        public UnitOfWork(
            ApiDbContext context,
            IServiceProvider serviceProvider,
            IBreedRepository breedRepository,
            IOrderRepository orderRepository,
            IPetRepository petRepository,
            IRoleRepository roleRepository,
            ISpeciesRepository speciesRepository,
            IUserRepository userRepository
            )
        {
            _context = context;
            _serviceProvider = serviceProvider;
            Breeds = breedRepository;
            Orders = orderRepository;
            Pets = petRepository;
            Roles = roleRepository;
            Species = speciesRepository;
            Users = userRepository;
        }

        public IBreedRepository Breeds { get; }
        public IOrderRepository Orders { get; }
        public IPetRepository Pets { get; }
        public IRoleRepository Roles { get; }
        public ISpeciesRepository Species { get; }
        public IUserRepository Users { get; }



        public void Dispose()
        {
            _context.Dispose();
        }

        public IGenericRepository<T> GenericRepository<T>() where T : class
        {
            if(_repositories.ContainsKey(typeof( T )))
            {
                return (IGenericRepository<T>)_repositories[typeof(T)];
            }
            var repository = _serviceProvider.GetRequiredService<IGenericRepository<T>>();
            _repositories[typeof(T)] = repository;
            return repository;
        }

        public async Task<int> SaveAsync()
        {
           return await _context.SaveChangesAsync();
        }
    }
}
