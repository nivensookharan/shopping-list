using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingList.Contracts;
using ShoppingList.ViewModels;
using ShoppingList.WebApi.Helpers;

namespace ShoppingList.WebApi.Controllers
{
    [Tags("2. Shopping List")]
    [ApiController]
    [Route("shopping-list")]
    public class ShoppingListController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ShoppingListController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets All the Shopping Lists for the Logged in User
        /// </summary>
        [HttpGet]
        [Authorize]
        [Route(template: "list", Order = 1)]
        public async Task<IEnumerable<ShoppingListViewModel>> Get()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var userId = await UserHelper.GetUserIdFromToken(token);
            return await new Core.ShoppingList(_unitOfWork, _mapper).GetListByUserId(userId);
        }

        /// <summary>
        /// Create a shopping list for the Logged in User
        /// </summary>
        [HttpPost]
        [Authorize]
        [Route(template: "add", Order = 2)]
        public async Task<ShoppingListViewModel> Add([FromBody] ShoppingListViewModel shoppingList)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var userId = await UserHelper.GetUserIdFromToken(token);
            shoppingList.UserId = userId;
            return await new Core.ShoppingList(_unitOfWork, _mapper).Add(shoppingList);
        }

        /// <summary>
        /// Updates a shopping list for the Logged in User
        /// </summary>
        [HttpPost]
        [Authorize]
        [Route(template: "update", Order = 3)]
        public async Task<ShoppingListViewModel> Update([FromBody] ShoppingListViewModel shoppingList)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var userId = await UserHelper.GetUserIdFromToken(token);
            shoppingList.UserId = userId;
            return await new Core.ShoppingList(_unitOfWork, _mapper).Update(shoppingList);
        }

        /// <summary>
        /// Deletes a shopping list for the Logged in User by Shopping List Id
        /// </summary>
        [HttpPost]
        [Authorize]
        [Route(template: "delete", Order = 4)]
        public async Task<bool> Delete(Guid shoppingListId)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var userId = await UserHelper.GetUserIdFromToken(token);
            // Validate User has access to the Shopping List
            return await new Core.ShoppingList(_unitOfWork, _mapper).Delete(shoppingListId);
        }



    }
}
