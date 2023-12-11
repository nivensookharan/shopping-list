using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ShoppingList.Contracts
{
    public  interface IRepositoryProvider
    {
       DbContext DbContext { get; set; }

        IRepository<T> GetRepositoryForEntityType<T>() where T : class;

        T GetRepository<T>(Func<DbContext, object> factory = null) where T : class;

        void SetRepository<T>(T repository);
    }
}
