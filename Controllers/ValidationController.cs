using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWT_Token_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidationController : ControllerBase
    {
        private readonly IJWTTokenHelper _jwtTokenHelper;
        public ValidationController(IJWTTokenHelper jwtTokenHelper)
        {
            _jwtTokenHelper = jwtTokenHelper;
        }

        [AllowAnonymous, HttpPost, Route("Authenticate")]
        public IActionResult Authenticate(UserDTO user)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Status = false,
                    Message = string.Join(Environment.NewLine, ModelState.Values
                                          .SelectMany(x => x.Errors)
                                          .Select(x => x.ErrorMessage))
                });
            }

            if (user.Email != "dummy123@gmail.com" && user.Password != "Password@123")
            {
                return BadRequest(new { Status = false, Message = "Invalid details" });
            }

            user.Id = 1;
            user.Role = "Admin";

            string token = _jwtTokenHelper.JWTTokenGenerator(user);

            return Ok(new
            {
                Status = true,
                Message = "Success",
                Data = new
                {
                    Id = user.Id,
                    Email = user.Email,
                    Role = user.Role,
                    Token = token
                }
            });
        }

        [Authorize(Roles = "Admin"), HttpGet, Route("CheckAuthentication")]
        public IActionResult CheckAuthentication()
        {
            return Ok(new
            {
                Status = true,
                Message = "Success",

                Data = new
                {
                    Id = User.Identity.Name,
                    Email = User.Claims.Where(x => x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")).FirstOrDefault().Value,
                    Role = User.Claims.Where(x => x.Type.Equals("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")).FirstOrDefault().Value
                }
            });
        }
    }
}
