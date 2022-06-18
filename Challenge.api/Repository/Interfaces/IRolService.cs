using Shared.shared.Dtos;

namespace Challenge.api.Repository.Interfaces
{
    public interface IRolService
    {
        Task<IEnumerable<RolDto>> GetAll();
    }
}
