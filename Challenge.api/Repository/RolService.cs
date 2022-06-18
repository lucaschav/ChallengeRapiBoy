using AutoMapper;
using Challenge.api.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.api.Context;
using Shared.shared.Dtos;

namespace Challenge.api.Repository
{
    public class RolService : IRolService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public RolService(ApplicationDbContext dbContext, IMapper mapper)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }
        public async Task<IEnumerable<RolDto>> GetAll()
        {
            var lst = await _dbContext.Rol.ToListAsync();
            return _mapper.Map<IEnumerable<RolDto>>(lst);
        }
    }
}
