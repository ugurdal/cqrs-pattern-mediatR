using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models.Token;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace DotnetCoreApiSample.Server.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("token")]
    public class TokenController : Controller
    {
        [HttpPost]
        public IActionResult Index(TokenRequest input)
        {
            //validate and check user
            if (1 == 0)
                return Unauthorized();

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Email, input.Username),
                new Claim(JwtRegisteredClaimNames.Exp, DateTime.Now.AddDays(1).ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, input.AccountId.ToString())
            };

            var securtyKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.SecurtyKey));
            var token = new JwtSecurityToken(
                issuer: Constants.SecurtyIssuer,
                audience: Constants.SecurtyAudience,
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: new SigningCredentials(securtyKey, SecurityAlgorithms.HmacSha256)
            );
            var access_token = new JwtSecurityTokenHandler().WriteToken(token);

            var response = Services.Response.Success(new TokenModel
            {
                Token = access_token,
                ValidThrough = DateTime.Now.AddDays(1)
            }, "");

            return Ok(response);
        }
    }
}