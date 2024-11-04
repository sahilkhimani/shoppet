using PetShopApi.Data;
using shoppetApi.Interfaces;
using System.Runtime.CompilerServices;

namespace shoppetApi.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApiDbContext _context;
        public UnitOfWork(
            ApiDbContext context,
            IBreedRepository breedRepository,
            IOrderRepository orderRepository,
            IPetRepository petRepository,
            IRoleRepository roleRepository,
            ISpeciesRepository speciesRepository,
            IUserRepository userRepository
            )
        {
            _context = context;
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

        public async Task<int> SaveAsync()
        {
           return await _context.SaveChangesAsync();
        }
    }
}
