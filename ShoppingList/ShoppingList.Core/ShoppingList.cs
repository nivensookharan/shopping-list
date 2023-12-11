using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ShoppingList.Contracts;
using ShoppingList.ViewModels;

namespace ShoppingList.Core
{
    public class ShoppingList: UnitOfWork
    {
        public ShoppingList(IUnitOfWork unitOfWork, IMapper mapper)
        {
            UnitofWork = unitOfWork;
            Mapper = mapper;
        }





        public async Task<ShoppingListViewModel> Add(ShoppingListViewModel shoppingListViewModel)
        {
            var shoppingList = Mapper.Map<Domain.ShoppingList>(shoppingListViewModel);
            shoppingList.ShoppingListId = Guid.NewGuid();
            shoppingList.CreatedDateTimeStamp = DateTime.Now;
            shoppingList.LastUpdatedDateTimeStamp = DateTime.Now;
            shoppingList.IsDeleted = false;
            UnitofWork.ShoppingLists.Add(shoppingList);
            UnitofWork.Commit();
            UnitofWork.ShoppingLists.Detach(shoppingList);
            return shoppingListViewModel;
        }

        public async Task<ShoppingListViewModel> Update(ShoppingListViewModel shoppingListViewModel)
        {
            var shoppingList = Mapper.Map<Domain.ShoppingList>(shoppingListViewModel);
            UnitofWork.ShoppingLists.Update(shoppingList, shoppingList.ShoppingListId);
            UnitofWork.Commit();
            UnitofWork.ShoppingLists.Detach(shoppingList);
            return shoppingListViewModel;
        }

        public async Task<bool> Delete(Guid shoppingListId)
        {
            
            UnitofWork.ShoppingLists.Delete(shoppingListId);
            UnitofWork.Commit();
            return true;
        }

        public async Task<IEnumerable<ShoppingListViewModel>> GetListByUserId(Guid userId)
        {
            var shoppingLists = UnitofWork.ShoppingLists.GetAllFilteredNoTrack(new List<string>() { "ShoppingListItems" }, x => x.UserId == userId);
            var shoppingListViewModels = Mapper.Map<IEnumerable<Domain.ShoppingList>, IEnumerable<ShoppingListViewModel>>(shoppingLists);
            return shoppingListViewModels;
        }


    }
}
