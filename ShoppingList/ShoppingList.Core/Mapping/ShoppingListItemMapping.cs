using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ShoppingList.ViewModels;

namespace ShoppingList.Core.Mapping
{
    public class ShoppingListItemMapping: Profile
    {
        public ShoppingListItemMapping()
        {
            CreateMap<Domain.ShoppingListItem, ShoppingListItemViewModel>()
                .ForMember(destination => destination.ShoppingListItemId,
                                       options => options.MapFrom(source => source.ShoppingListItemId))
                .ForMember(destination => destination.ShoppingListId,
                                       options => options.MapFrom(source => source.ShoppingListId))
                .ForMember(destination => destination.UserId, options => options.MapFrom(source => source.UserId))
                .ForMember(destination => destination.DisplayName,
                                       options => options.MapFrom(source => source.DisplayName))
                .ForMember(destination => destination.Description,
                                       options => options.MapFrom(source => source.Description))
                .ForMember(destination => destination.ImageUrl, options => options.MapFrom(source => source.ImageUrl))
                .ForMember(destination => destination.LastUpdatedDateTimeStamp,
                                       options => options.MapFrom(source => source.LastUpdatedDateTimeStamp))
                .ForMember(destination => destination.CreatedDateTimeStamp,
                                       options => options.MapFrom(source => source.CreatedDateTimeStamp))
                .ForMember(destination => destination.IsDeleted, options => options.MapFrom(source => source.IsDeleted))
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter();

            CreateMap<ShoppingListItemViewModel, Domain.ShoppingListItem>()
                .ForMember(destination => destination.ShoppingListItemId,
                                       options => options.MapFrom(source => source.ShoppingListItemId))
                .ForMember(destination => destination.ShoppingListId,
                                       options => options.MapFrom(source => source.ShoppingListId))
                .ForMember(destination => destination.UserId, options => options.MapFrom(source => source.UserId))
                .ForMember(destination => destination.DisplayName,
                                       options => options.MapFrom(source => source.DisplayName))
                .ForMember(destination => destination.Description,
                                       options => options.MapFrom(source => source.Description))
                .ForMember(destination => destination.ImageUrl, options => options.MapFrom(source => source.ImageUrl))
                .ForMember(destination => destination.LastUpdatedDateTimeStamp,
                                       options => options.MapFrom(source => source.LastUpdatedDateTimeStamp))
                .ForMember(destination => destination.CreatedDateTimeStamp,
                                       options => options.MapFrom(source => source.CreatedDateTimeStamp))
                .ForMember(destination => destination.IsDeleted, options => options.MapFrom(source => source.IsDeleted))
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
        }
    }
}
