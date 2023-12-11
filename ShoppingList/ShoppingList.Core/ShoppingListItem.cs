using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Minio;
using Minio.DataModel.Args;
using Minio.DataModel.Response;
using ShoppingList.Contracts;
using ShoppingList.ViewModels;

namespace ShoppingList.Core
{
    public class ShoppingListItem : UnitOfWork
    {
        public ShoppingListItem(IUnitOfWork unitOfWork, IMapper mapper, IMinioClient minioClient)
        {
            UnitofWork = unitOfWork;
            Mapper = mapper;
            MinioClient = minioClient;
        }

        public async Task<ShoppingListItemViewModel> Add(ShoppingListItemViewModel shoppingListItemViewModel)
        {
            if (!await IsUserAuthorized(shoppingListItemViewModel.ShoppingListId, shoppingListItemViewModel.UserId))
            {
                throw new Exception("User is not authorized to add items to this shopping list");
            }
            var shoppingListItem = Mapper.Map<Domain.ShoppingListItem>(shoppingListItemViewModel);
            UnitofWork.ShoppingListItems.Add(shoppingListItem);
            UnitofWork.Commit();
            UnitofWork.ShoppingListItems.Detach(shoppingListItem);
            return shoppingListItemViewModel;
        }

        public async Task<ShoppingListItemViewModel> Update(ShoppingListItemViewModel shoppingListItemViewModel)
        {
            if (!await IsUserAuthorized(shoppingListItemViewModel.ShoppingListId, shoppingListItemViewModel.UserId))
            {
                throw new Exception("User is not authorized to Update items to this shopping list");
            }
            var shoppingListItem = Mapper.Map<Domain.ShoppingListItem>(shoppingListItemViewModel);
            UnitofWork.ShoppingListItems.Update(shoppingListItem, shoppingListItem.ShoppingListItemId);
            UnitofWork.Commit();
            UnitofWork.ShoppingListItems.Detach(shoppingListItem);
            return shoppingListItemViewModel;
        }

        public async Task<bool> Delete(Guid shoppingListItemId, Guid userId)
        {
            if (!await IsUserAuthorized(shoppingListItemId, userId))
            {
                throw new Exception("User is not authorized to Delete items From this shopping list");
            }
            UnitofWork.ShoppingListItems.Delete(shoppingListItemId);
            UnitofWork.Commit();
            return true;
        }

        public async Task<IEnumerable<ShoppingListItemViewModel>> GetListByShoppingListId(Guid shoppingListId, Guid userId)
        {
            if (!await IsUserAuthorized(shoppingListId, userId))
            {
                throw new Exception("User is not authorized to get items from this shopping list");
            }
            var shoppingListItems = UnitofWork.ShoppingListItems.GetAllFilteredNoTrack(new List<string>() { "ShoppingList" }, x => x.ShoppingListId == shoppingListId);
            var shoppingListItemViewModels = Mapper.Map<IEnumerable<Domain.ShoppingListItem>, IEnumerable<ShoppingListItemViewModel>>(shoppingListItems);
            return shoppingListItemViewModels;
        }

        public async Task<bool> UploadImage(Stream file, string fileName, string contentType, Guid shoppingListItemId, Guid userId)
        {
            try
            {
                var shoppingListItem = UnitofWork.ShoppingListItems.GetByID(shoppingListItemId);
                if (shoppingListItem == null)
                {
                    throw new Exception("Shopping List Item does not exist");
                }
                if (!await IsUserAuthorized(shoppingListItem.ShoppingListId, userId))
                {
                    throw new Exception("User is not authorized to upload images to this shopping list");
                }
                var bucketName = "games-global";
                var bucket = new BucketExistsArgs().WithBucket(bucketName);
                bool doesBucketExist = await MinioClient.BucketExistsAsync(bucket).ConfigureAwait(false);
                if (!doesBucketExist)
                {
                    var makeBucket = new MakeBucketArgs().WithBucket(bucketName);
                    await MinioClient.MakeBucketAsync(makeBucket).ConfigureAwait(false);
                }
                var minioFileBuilder = new PutObjectArgs()
                   .WithBucket(bucketName)
                   .WithObject(fileName)
                   .WithStreamData(file)
                   .WithObjectSize(file.Length)
                   .WithContentType(contentType);

                var result = await MinioClient.PutObjectAsync(minioFileBuilder).ConfigureAwait(false);
                if (result != null)
                {
                    var _shoppingListItem = UnitofWork.ShoppingListItems.GetByID(shoppingListItemId);
                    var baseUrl = MinioClient.Config.Endpoint.ToString();
                    _shoppingListItem.ImageUrl = $"{baseUrl}/{bucketName}/{fileName}";
                    UnitofWork.ShoppingListItems.Update(_shoppingListItem, _shoppingListItem.ShoppingListItemId);
                    UnitofWork.Commit();
                    UnitofWork.ShoppingListItems.Detach(_shoppingListItem);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async Task<bool> IsUserAuthorized(Guid shoppingListId, Guid userId)
        {
            var shoppingList = UnitofWork.ShoppingLists.GetByID(shoppingListId);
            if (shoppingList.UserId == userId)
            {
                return true;
            }
            return false;
        }
    }
}
