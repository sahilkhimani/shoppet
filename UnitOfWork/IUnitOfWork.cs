using shoppetApi.Interfaces;

namespace shoppetApi.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IBreedRepository Breeds { get; }
        IOrderRepository Orders { get; }
        IPetRepository Pets { get; }
        IRoleRepository Roles { get; }
        ISpeciesRepository Species { get; }
        IUserRepository Users { get; }
        IGenericRepository<T> GenericRepository<T>() where T : class;

        Task<int> SaveAsync();


    }
}
