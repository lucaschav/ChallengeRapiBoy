using Challenge.api.Repository.Interfaces;
using Challenge.Api.Models;
using Challenge.shared.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shared.shared.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Challenge.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {

        public static UsuarioDto user = new UsuarioDto();
        private readonly IConfiguration _configuration;
        private readonly IUsuarioService _usuarioService;

        public AuthController(IConfiguration configuration, IUsuarioService usuarioService)
        {
            this._configuration = configuration;
            this._usuarioService = usuarioService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            try
            {
                await _usuarioService.Register(model);
                return Ok(true);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }
        [Authorize(Roles = "admin")]
        [HttpPut("Update")]
        public async Task<IActionResult> Update(UsuarioDto model)
        {
            try
            {
                await _usuarioService.Update(model);
                return Ok(true);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }
        [Authorize(Roles = "admin")]
        [HttpDelete]
        [Route("Delete/{usuarioId}")]
        public async Task<IActionResult> delete(int usuarioId)
        {
            try
            {
                await _usuarioService.Delete(usuarioId);
                return Ok(true);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }
        [Authorize]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _usuarioService.GetAll();
                return Ok(result);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]LoginRequest model)
        {
            try
            {
                var result = await _usuarioService.Login(model);

                user = result;

                string token = CreateToken(user);

                user.Token = token;

                var refreshToken = GenerateRefreshToken();
                SetRefreshToken(refreshToken);


                return Ok(user);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };

            return refreshToken;
        }

        private void SetRefreshToken(RefreshToken newRefreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;
        }

        private string CreateToken(UsuarioDto userDTO)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, userDTO.Email.ToString()),
                new Claim("UsuarioId", userDTO.Id.ToString()),
                new Claim(ClaimTypes.Role, userDTO.Rol.Nombre)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(10),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
