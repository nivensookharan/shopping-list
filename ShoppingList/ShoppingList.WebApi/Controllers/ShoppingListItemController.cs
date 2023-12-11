using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Minio;
using ShoppingList.Contracts;
using ShoppingList.ViewModels;
using ShoppingList.WebApi.Helpers;


namespace ShoppingList.WebApi.Controllers
{
    [Tags("3. Shopping List Items")]
    [ApiController]
    [Route("shopping-list-item")]
    
    public class ShoppingListItemController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMinioClient _minioClient;

        public ShoppingListItemController(IUnitOfWork unitOfWork, IMapper mapper, IMinioClient minioClient)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _minioClient = minioClient;
        }

        /// <summary>
        /// Gets All the Shopping List Items for the Logged in User
        /// </summary>
        [HttpGet]
        [Authorize]
        [Route(template: "list/{shoppingListId}", Order = 1)]
        public async Task<IEnumerable<ShoppingListItemViewModel>> Get(Guid shoppingListId)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var userId = await UserHelper.GetUserIdFromToken(token);
            return await new Core.ShoppingListItem(_unitOfWork, _mapper, _minioClient).GetListByShoppingListId(shoppingListId, userId);
        }


        /// <summary>
        /// Adds an item to a shopping list for the Logged in User
        /// </summary>
        [HttpPost]
        [Authorize]
        [Route(template: "add", Order = 2)]
        public async Task<ShoppingListItemViewModel> Add([FromBody] ShoppingListItemViewModel shoppingListItem)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var userId = await UserHelper.GetUserIdFromToken(token);
            shoppingListItem.UserId = userId;
            return await new Core.ShoppingListItem(_unitOfWork, _mapper, _minioClient).Add(shoppingListItem);
        }

        /// <summary>
        /// Updates an item from a shopping list for the Logged in User
        /// </summary>
        [HttpPost]
        [Authorize]
        [Route(template: "update", Order = 3)]
        public async Task<ShoppingListItemViewModel> Update([FromBody] ShoppingListItemViewModel shoppingListItem)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var userId = await UserHelper.GetUserIdFromToken(token);
            shoppingListItem.UserId = userId;
            return await new Core.ShoppingListItem(_unitOfWork, _mapper, _minioClient).Update(shoppingListItem);
        }

        /// <summary>
        /// Deletes an item from a shopping list for the Logged in User
        /// </summary>
        [HttpPost]
        [Authorize]
        [Route(template: "delete", Order = 4)]
        public async Task<bool> Delete([FromBody] Guid shoppingListItemId)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var userId = await UserHelper.GetUserIdFromToken(token);
            return await new Core.ShoppingListItem(_unitOfWork, _mapper, _minioClient).Delete(shoppingListItemId, userId);
        }

        /// <summary>
        /// Add / Update Image for a Shopping List Item
        /// </summary>
        [HttpPost]
        [Authorize]
        [Route(template: "image/{shoppingListItemId}", Order = 5)]
        public async Task<IActionResult> AddUpdateImage(IFormFile file, Guid shoppingListItemId)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var userId = await UserHelper.GetUserIdFromToken(token);
            var fileType = file.ContentType;
            var fileExtension = MimeTypes.MimeTypeMap.GetExtension(fileType);
            await using (var stream = file.OpenReadStream())
            {
                var result = await new Core.ShoppingListItem(_unitOfWork, _mapper, _minioClient).UploadImage(stream, $"{shoppingListItemId+fileExtension}",fileType, shoppingListItemId, userId);
                if (result)
                {
                    return Ok();
                }
                else
                {
                    return Unauthorized();
                }
            }

        }
    }
}
