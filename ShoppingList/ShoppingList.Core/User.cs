using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ShoppingList.Contracts;
using ShoppingList.Domain;
using ShoppingList.ViewModels;

namespace ShoppingList.Core
{
    public class User: UnitOfWork
    {
        public User(IUnitOfWork unitOfWork, IMapper mapper)
        {
            UnitofWork = unitOfWork;
            Mapper = mapper;
        }
        public async Task<IEnumerable<ViewModels.UserViewModel>>GetAll()
        {
            var users = await UnitofWork.Users.GetAll();
            var userList = Mapper.Map<IEnumerable<Domain.User>, IEnumerable<ViewModels.UserViewModel>>(users);
            return await Task.FromResult(userList);
        }

        public Task<UserViewModel> GetProfile(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtTokenObject = tokenHandler.ReadToken(token) as JwtSecurityToken;
            var user = UnitofWork.Users.GetByID(Guid.Parse(jwtTokenObject?.Subject ?? throw new InvalidOperationException()));
            var userViewModel = Mapper.Map<Domain.User, ViewModels.UserViewModel>(user);
            return Task.FromResult(userViewModel);
        }

        public Task<bool> ValidateUser(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtTokenObject = tokenHandler.ReadToken(token) as JwtSecurityToken;
                var existingUser = UnitofWork.Users.GetByID(Guid.Parse(jwtTokenObject?.Subject ?? throw new InvalidOperationException()));
                if (existingUser == null)
                {
                    // Change to Auto Mapper
                    var userDto = new Domain.User()
                    {
                        UserId = Guid.Parse(jwtTokenObject.Subject),
                        Username = jwtTokenObject.Claims.FirstOrDefault(x => x.Type == "preferred_username")?.Value!,
                        EmailAddress = jwtTokenObject.Claims.FirstOrDefault(x => x.Type == "email")?.Value!,
                        Firstname = jwtTokenObject.Claims.FirstOrDefault(x => x.Type == "given_name")?.Value!,
                        Lastname = jwtTokenObject.Claims.FirstOrDefault(x => x.Type == "family_name")?.Value!,
                        MobileNumber = "",
                        // Change to Enum
                        UserRole = 1,
                        LastTokenValue = token,
                        LastLoggedInDateTimeStamp = DateTime.Now.ToUniversalTime(),
                        CreatedDateTimeStamp = DateTime.Now.ToUniversalTime(),
                        LastUpdatedDateTimeStamp = DateTime.Now.ToUniversalTime(),
                        IsDeleted = false,
                    };
                    try
                    {
                        UnitofWork.Users.Add(userDto);
                        UnitofWork.Commit();
                        UnitofWork.Users.Detach(userDto);
                    }
                    catch (Exception ex)
                    {
                        UnitofWork.Users.Detach(userDto);
                        return Task.FromResult(false);
                    }
                }
                else
                {
                    // Change to AutoMapper
                    existingUser.Username = jwtTokenObject.Claims.FirstOrDefault(x => x.Type == "preferred_username")?.Value!;
                    existingUser.EmailAddress = jwtTokenObject.Claims.FirstOrDefault(x => x.Type == "email")?.Value!;
                    existingUser.Firstname = jwtTokenObject.Claims.FirstOrDefault(x => x.Type == "given_name")?.Value!;
                    existingUser.Lastname = jwtTokenObject.Claims.FirstOrDefault(x => x.Type == "family_name")?.Value!;
                    existingUser.UserRole = 1;
                    existingUser.LastUpdatedDateTimeStamp = DateTime.Now.ToUniversalTime();
                    existingUser.LastTokenValue = token;
                    existingUser.LastLoggedInDateTimeStamp = DateTime.Now.ToUniversalTime();
                    try
                    {
                        UnitofWork.Users.Update(existingUser);
                        UnitofWork.Commit();
                        UnitofWork.Users.Detach(existingUser);
                    }
                    catch (Exception ex)
                    {
                        UnitofWork.Users.Detach(existingUser);
                        return Task.FromResult(false);
                    }
                }
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                return Task.FromResult(false);
            }
        }
    }
}
