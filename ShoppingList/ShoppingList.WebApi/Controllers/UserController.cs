using AutoMapper;
using AutoMapper.QueryableExtensions.Impl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingList.Contracts;
using ShoppingList.ViewModels;

namespace ShoppingList.WebApi.Controllers
{
    [Tags("1. User")]
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// With the token provided, validate if the user is currently associated with the system
        /// Creates the user if the user does not exist
        /// Updates the user if the user does exist
        /// </summary>
        [HttpGet]
        [Authorize]
        [Route(template: "validate", Order = 1)]
        [ProducesResponseType(typeof(bool), 200, "application/json")]
        [ProducesResponseType(type: typeof(string),  statusCode: 401, "application/json") ]
        [ProducesResponseType(type: typeof(string), statusCode: 500, "application/json")]
        public async Task<IActionResult> Validate()
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var isUserValid = await new Core.User(_unitOfWork, _mapper).ValidateUser(token);
                if (!isUserValid)
                {
                    return Unauthorized();
                }
                return Ok(isUserValid);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Get the User Profile based on Authorization
        /// </summary>
        [HttpGet]
        [Authorize]
        [Route(template: "profile", Order = 2)]
        [ProducesResponseType(typeof(UserViewModel), 200, "application/json")]
        [ProducesResponseType(typeof(string),statusCode: 401, "application/json")]
        [ProducesResponseType(type: typeof(string), statusCode: 500, "application/json")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var profile = await new Core.User(_unitOfWork, _mapper).GetProfile(token);
                return Ok(profile);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        /// <summary>
        /// List all users registered on the System
        /// </summary>
        [HttpGet]
        [Authorize]
        [Route(template: "list",  Order = 3)]
        [ProducesResponseType(typeof(IEnumerable<UserViewModel>), 200, "application/json")]
        [ProducesResponseType(typeof(string), statusCode: 401, "application/json")]
        [ProducesResponseType(type: typeof(string), statusCode: 500, "application/json")]
        public async Task<IActionResult> List()
        {
            try
            {
                var users = await new Core.User(_unitOfWork, _mapper).GetAll();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

   


    }
}
