using API.DataAccess.Interfaces;
using System.Collections;

namespace API.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FindmeContext _context;
        private Hashtable _repositories;

        public UnitOfWork(FindmeContext context)
        {
            _context = context;
        }

        public IBaseRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            if (_repositories == null) _repositories = new Hashtable();
            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(BaseRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);
                _repositories.Add(type, repositoryInstance);
            }

            return (IBaseRepository<TEntity>)_repositories[type];
        }
        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
