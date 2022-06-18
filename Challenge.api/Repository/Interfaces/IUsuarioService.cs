using Challenge.shared.RequestModels;
using Shared.shared.Dtos;

namespace Challenge.api.Repository.Interfaces
{
    public interface IUsuarioService
    {
        Task<UsuarioDto> Login(LoginRequest loginRequest);
        Task Register(RegisterRequest usuarioDto);
        Task Update(UsuarioDto usuarioDto);
        Task Delete(int usuarioId);
        Task<IEnumerable<UsuarioDto>> GetAll();
    }
}
