using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingList.Domain;

namespace ShoppingList.Contracts
{
    public interface IUnitOfWork
    {

        void Commit();
        IRepository<User> Users { get; }
        IRepository<Domain.ShoppingList> ShoppingLists { get; }
        IRepository<ShoppingListItem> ShoppingListItems { get; }

    }
}
