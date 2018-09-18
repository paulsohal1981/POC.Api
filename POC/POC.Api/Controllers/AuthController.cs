using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace POC.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class AuthController : BaseController
    {
        private readonly IConfiguration _configuration;
        public AuthController(IMapper mapper, IConfiguration configuration):base(mapper)
        {
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("token")]
        public IActionResult Token()
        {

            var audienceConfig = _configuration.GetSection("JWTAuth");
            var symmetricKeyAsBase64 = audienceConfig["Secret"];
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var validIssuer = audienceConfig["Issuer"];
            var ValidAudience = audienceConfig["Audience"];
            var signInCred = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature);

            var claims = new Claim[] {new Claim(ClaimTypes.Name, "Paul") };

            var token = new JwtSecurityToken(
                issuer: validIssuer,
                audience: ValidAudience,
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