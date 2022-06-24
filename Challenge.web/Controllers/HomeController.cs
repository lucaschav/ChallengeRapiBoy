using AutoMapper;
using Challenge.shared.RequestModels;
using Challenge.web.Models;
using Challenge.web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shared.shared.Dtos;
using System.Diagnostics;

namespace Challenge.web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUsuarioService _usuarioService;
        private readonly IRolService _rolService;

        public HomeController(ILogger<HomeController> logger, IUsuarioService usuarioService, IRolService rolService)
        {
            _logger = logger;
            this._usuarioService = usuarioService;
            this._rolService = rolService;
        }

        public async Task<IActionResult> Index(bool mostrarTodo = false)
        {
            if(mostrarTodo)
                TempData["mostrarTodo"] = mostrarTodo;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> _CreateOrEdit(int usuarioId, string email, int rolId, bool estado)
        {
            UsuarioDto usuarioDto = new UsuarioDto();

            if (usuarioId != 0)
            {
                usuarioDto.Id = usuarioId;
                usuarioDto.Email = email;
                usuarioDto.RolId = rolId;
                usuarioDto.Activo = estado;
            }

            var roles = await _rolService.GetRoles(User.Claims.Where(c => c.Type == "token").First().Value);

            ViewBag.rolesLst = new SelectList(roles, nameof(RolDto.Id), nameof(RolDto.Nombre));

            return PartialView(usuarioDto);

        }

        [HttpPost]
        public async Task<JsonResult> _CreateOrEdit(int usuarioId, UsuarioDto usuarioDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    usuarioDto.Id = usuarioId;
                    var modelErrors = new List<string>();
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var modelError in modelState.Errors)
                        {
                            modelErrors.Add(modelError.ErrorMessage);
                        }
                    }
                    return Json(new {result = false, errorMsg = modelErrors.First()});
                }

                if (usuarioId == 0)
                {
                    RegisterRequest registerRequest = new RegisterRequest()
                    {
                        PasswordString = usuarioDto.PasswordString,
                        ConfirmPassword = usuarioDto.ConfirmPassword,
                        RolId = usuarioDto.RolId,
                        Email = usuarioDto.Email,
                        Activo = usuarioDto.Activo
                    };

                    await _usuarioService.Register(registerRequest);
                    TempData["createOrEdit"] = "create";
                }
                else
                {
                    usuarioDto.Id = usuarioId;
                    await _usuarioService.Update(usuarioDto, User.Claims.Where(c => c.Type == "token").First().Value);
                    TempData["createOrEdit"] = "update";
                }

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                return Json(new { result = false, errorMsg = ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int usuarioId)
        {
            var result = await _usuarioService.Delete(usuarioId, User.Claims.Where(c => c.Type == "token").First().Value);

            return Json(new { result = result });
        }

        [HttpPost]
        public async Task<JsonResult> UpdateEstado(int usuarioId, int rolId, string email, bool activo)
        {
            var result = await _usuarioService.Update(new UsuarioDto()
            {
                Id = usuarioId,
                Email = email,
                RolId = rolId,
                Activo = activo
            }, User.Claims.Where(c => c.Type == "token").First().Value);

            return Json(new { result = result });
        }

        public async Task<IActionResult> _TablaUsuarios()
        {
            bool mostrarTodo = TempData["mostrarTodo"] != null ? true : false;

            var result = await _usuarioService.GetUsuarios(User.Claims.Where(c => c.Type == "token").First().Value);

            if (!mostrarTodo || (mostrarTodo && User.IsInRole("cliente")))
                result = result.Where(x => x.Rol.Nombre == "cliente").ToList();

            return PartialView(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}