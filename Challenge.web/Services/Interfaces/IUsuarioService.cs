using Challenge.shared.RequestModels;
using Microsoft.AspNetCore.Mvc;
using Shared.shared.Dtos;
using System.Security.Claims;

namespace Challenge.web.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<List<Claim>> Login(LoginRequest loginRequest);
        Task<bool> Register(RegisterRequest usuarioDto);
        Task<IEnumerable<UsuarioDto>> GetUsuarios(string token);
        Task<bool> Update(UsuarioDto usuarioDto, string token);
        Task<bool> Delete(int usuarioId, string token);
    }
}
