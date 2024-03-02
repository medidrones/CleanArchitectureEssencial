using CleanArchMvc.API.Models;
using CleanArchMvc.Domain.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CleanArchMvc.API.Controllers
{
    [ApiController]    
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly IAuthenticate _authentication;
        private readonly IConfiguration _configuration;

        public TokenController(IAuthenticate authentication, IConfiguration configuration)
        {
            _authentication = authentication ??
                throw new ArgumentNullException(nameof(authentication));
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("LoginUser")]
        public async Task<ActionResult<UserToken>> Login([FromBody] LoginModel userInfo)
        {
            var result = await _authentication.Authenticate(userInfo.Email, userInfo.Password);

            if (result)
            {
                return GenerateToken(userInfo);               
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid Login attempt.");

                return BadRequest(ModelState);
            }
        }

        [Authorize]
        [HttpPost("CreateUser")]
        [ApiExplorerSettings(IgnoreApi = true)]        
        public async Task<ActionResult> CreateUser([FromBody] LoginModel userInfo)
        {
            var result = await _authentication.RegisterUser(userInfo.Email, userInfo.Password);

            if (result)
            {                
                return Ok($"User {userInfo.Email} login successfully.");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid Login attempt.");

                return BadRequest(ModelState);
            }
        }

        private UserToken GenerateToken(LoginModel userInfo)
        {
            // Declarações do usuário
            var claims = new[]
            {
                new Claim("email", userInfo.Email),
                new Claim("meuValor", "o que voce quiser"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Gerar chave privada para assinar o token
            var privateKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

            // Gerar a assinatura digital do token
            var credentials = new SigningCredentials(privateKey, SecurityAlgorithms.HmacSha256);

            // Definir o tempo de expiração do token
            var expiration = DateTime.UtcNow.AddMinutes(10);

            // Gerar o token
            JwtSecurityToken token = new JwtSecurityToken(                
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credentials);

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }
}
