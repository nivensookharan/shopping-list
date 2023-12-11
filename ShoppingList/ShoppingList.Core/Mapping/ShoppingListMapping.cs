using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ShoppingList.ViewModels;

namespace ShoppingList.Core.Mapping
{
    public class ShoppingListMapping : Profile
    {
        public ShoppingListMapping()
        {
            CreateMap<Domain.ShoppingList, ShoppingListViewModel>()
                .ForMember(destination => destination.ShoppingListId,
                    options => options.MapFrom(source => source.ShoppingListId))
                .ForMember(destination => destination.UserId, options => options.MapFrom(source => source.UserId))
                .ForMember(destination => destination.DisplayName,
                    options => options.MapFrom(source => source.DisplayName))
                .ForMember(destination => destination.Description,
                    options => options.MapFrom(source => source.Description))
                .ForMember(destination => destination.LastUpdatedDateTimeStamp,
                    options => options.MapFrom(source => source.LastUpdatedDateTimeStamp))
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter();



            CreateMap<ShoppingListViewModel, Domain.ShoppingList>()
                .ForMember(destination => destination.ShoppingListId,
                    options => options.MapFrom(source => source.ShoppingListId))
                .ForMember(destination => destination.UserId, options => options.MapFrom(source => source.UserId))
                .ForMember(destination => destination.DisplayName,
                    options => options.MapFrom(source => source.DisplayName))
                .ForMember(destination => destination.Description,
                    options => options.MapFrom(source => source.Description))
                .ForMember(destination => destination.LastUpdatedDateTimeStamp,
                    options => options.MapFrom(source => source.LastUpdatedDateTimeStamp))
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter();


        }
    }
}
