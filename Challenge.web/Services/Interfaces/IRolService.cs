using Challenge.shared.RequestModels;
using Microsoft.AspNetCore.Mvc;
using Shared.shared.Dtos;
using System.Security.Claims;

namespace Challenge.web.Services.Interfaces
{
    public interface IRolService
    {
        Task<IEnumerable<RolDto>> GetRoles(string token);
    }
}
