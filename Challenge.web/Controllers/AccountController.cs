using Challenge.shared.RequestModels;
using Challenge.web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Shared.shared.Dtos;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Challenge.web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IConfiguration _configuration;

        public AccountController(IUsuarioService usuarioService, IConfiguration configuration)
        {
            this._usuarioService = usuarioService;
            this._configuration = configuration;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(loginRequest);
                }

                if (loginRequest.Email == "correo@prueba.com")
                {
                    RegisterRequest usuarioDto = new RegisterRequest()
                    {
                        Email = _configuration["UsuarioInicial:email"],
                        PasswordString = _configuration["UsuarioInicial:password"],
                        ConfirmPassword = _configuration["UsuarioInicial:password"],
                        RolId = int.Parse(_configuration["UsuarioInicial:rolId"]),
                        Activo = true
                    };

                    try
                    {
                        var resultRegister = await _usuarioService.Register(usuarioDto);
                    }
                    catch
                    {}
                }

                var result = await _usuarioService.Login(loginRequest);

                if (result.Count() != 0)
                {
                    var claimsIdentity = new ClaimsIdentity(
                        result, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {

                        RedirectUri = "/Home/Index",

                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    return RedirectToAction("Index", "Home");
                }

                return View(loginRequest);
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                return View(loginRequest);
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}

