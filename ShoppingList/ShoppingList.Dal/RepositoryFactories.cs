using Microsoft.EntityFrameworkCore;
using ShoppingList.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Dal
{

    public class RepositoryFactories
    {
        public IDictionary<Type, Func<DbContext, object>> _repositoryFactories;

        private IDictionary<Type, Func<DbContext, object>> GetFEFactories()
        {
            return new Dictionary<Type, Func<DbContext, object>>
            {

            };
        }

        public RepositoryFactories()
        {
            _repositoryFactories = new Dictionary<Type, Func<DbContext, object>>(); //GetFEFactories();
        }

        public RepositoryFactories(IDictionary<Type, Func<DbContext, object>> factories)
        {
            _repositoryFactories = factories;
        }

        public Func<DbContext, object> GetRepositoryFactory<T>()
        {
            Func<DbContext, object> factory;
            _repositoryFactories.TryGetValue(typeof(T), out factory);
            return factory;
        }

        public Func<DbContext, object> GetRepositoryFactoryForEntityType<T>() where T : class
        {
            return GetRepositoryFactory<T>() ?? DefaultEntityRepositoryFactory<T>();
        }

        public Func<DbContext, object> DefaultEntityRepositoryFactory<T>() where T : class
        {
            return dbContext => new Repository<T>(dbContext);
        }
    }
}
