using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ShoppingList.ViewModels;


namespace ShoppingList.Core.Mapping
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<Domain.User, UserViewModel>()
                
                .ForMember(destination => destination.EmailAddress,
                    options => options.MapFrom(source => source.EmailAddress))
                .ForMember(destination => destination.Username, options => options.MapFrom(source => source.Username))
                .ForMember(destination => destination.Firstname, options => options.MapFrom(source => source.Firstname))
                .ForMember(destination => destination.Lastname, options => options.MapFrom(source => source.Lastname))
                .ForMember(destination => destination.LastUpdatedDateTimeStamp,
                    options => options.MapFrom(source => source.LastUpdatedDateTimeStamp))
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter();


            CreateMap<UserViewModel, Domain.User>()
                
                .ForMember(destination => destination.EmailAddress, options => options.MapFrom(source => source.EmailAddress))
                .ForMember(destination => destination.Username, options => options.MapFrom(source => source.Username))
                .ForMember(destination => destination.Firstname, options => options.MapFrom(source => source.Firstname))
                .ForMember(destination => destination.Lastname, options => options.MapFrom(source => source.Lastname))
                .ForMember(destination => destination.LastUpdatedDateTimeStamp,
                    options => options.MapFrom(source => source.LastUpdatedDateTimeStamp))
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
        }
    }
}
