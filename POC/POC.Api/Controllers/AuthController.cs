using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace POC.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class AuthController : BaseController
    {
        public AuthController(IMapper mapper):base(mapper)
        {

        }

        [AllowAnonymous]
        [HttpPost("token")]
        public IActionResult Token()
        {


            var claims = new Claim[] {new Claim(ClaimTypes.Name, "Paul") };
            //TODO: This comes from a config file. 
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("fasfsadfasdfsdfasdfasdfsd"));
            var signInCred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                issuer:"mysite.com",
                audience:"mysite.com",
                expires: DateTime.Now.AddMinutes(5),
                claims: claims,
                signingCredentials: signInCred
                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(tokenString);


            //return Ok();
        }

    }
}