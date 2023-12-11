using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingList.Contracts;
using ShoppingList.Domain;

namespace ShoppingList.Dal
{
    public  class UnitOfWork: IUnitOfWork
    {
        private Entities DbContext { get; set; }
        protected IRepositoryProvider RepositoryProvider { get; set; }

        public UnitOfWork(IRepositoryProvider repositoryProvider)
        {
            CreateDbContext();

            repositoryProvider.DbContext = DbContext;
            RepositoryProvider = repositoryProvider;
        }

        protected void CreateDbContext()
        {
            DbContext = new Entities();
        }

        public void Commit()
        {
            DbContext.SaveChanges();
        }

        public IRepository<User> Users => GetStandardRepo<User>();
        public IRepository<Domain.ShoppingList> ShoppingLists => GetStandardRepo<Domain.ShoppingList>();
        public IRepository<ShoppingListItem> ShoppingListItems => GetStandardRepo<ShoppingListItem>();


        private IRepository<T> GetStandardRepo<T>() where T : class
        {
            return RepositoryProvider.GetRepositoryForEntityType<T>();
        }

        private T GetRepo<T>() where T : class
        {
            return RepositoryProvider.GetRepository<T>();
        }
    }
}
