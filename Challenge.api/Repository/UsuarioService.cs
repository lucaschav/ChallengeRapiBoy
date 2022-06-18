using AutoMapper;
using Challenge.api.Repository.Interfaces;
using Challenge.Api.Models;
using Challenge.shared.RequestModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shared.api.Context;
using Shared.Models;
using Shared.shared.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Challenge.api.Repository
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public UsuarioService(ApplicationDbContext dbContext, IMapper mapper)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public async Task Delete(int usuarioId)
        {
            try
            {
                var usuario = await _dbContext.Usuario.Where(u => u.Id == usuarioId).FirstOrDefaultAsync();

                if (usuario == null)
                    throw new Exception("El usuario a eliminar no existe");

                _dbContext.Usuario.Remove(usuario);
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(String.IsNullOrEmpty(ex.Message) ? "Ocurrio un problema" : ex.Message);
            }
        }

        public async Task<IEnumerable<UsuarioDto>> GetAll()
        {
            var lst = await _dbContext.Usuario
                .Include(u => u.Rol)
                .ToListAsync();
            return _mapper.Map<IEnumerable<UsuarioDto>>(lst);
        }

        public async Task<UsuarioDto> Login(LoginRequest model)
        {
            try
            {
                var userExist = await _dbContext.Usuario.Where(x => x.Email == model.Email).FirstOrDefaultAsync();

                if (userExist == null)
                {
                    throw new Exception("El correo electronico ingresado no se encuentra registrado");
                }

                if (!userExist.Activo)
                    throw new Exception("El usuario con el que intenta ingresar se encuentra desactivado");

                if (!VerifyPasswordHash(model.Password, userExist.Password, userExist.PasswordSalt))
                {
                    throw new Exception("Contraseña incorrecta");
                }

                userExist.Rol = await _dbContext.Rol.Where(x => x.Id == userExist.RolId).FirstAsync();

                var userDTO = _mapper.Map<UsuarioDto>(userExist);

                return userDTO;
            }
            catch (Exception ex)
            {
                throw new Exception(String.IsNullOrEmpty(ex.Message) ? "Ocurrio un problema" : ex.Message);
            }
        }

        public async Task Register(RegisterRequest model)
        {
            try
            {
                var emailExist = await _dbContext.Usuario.Where(x => x.Email == model.Email).AnyAsync();

                if (emailExist)
                    throw new Exception("El Email ingresado ya se encuentra registrado");

                CreatePasswordHash(model.PasswordString, out byte[] passwordHash, out byte[] passwordSalt);

                var usuario = _mapper.Map<Usuario>(model);

                usuario.Password = passwordHash;
                usuario.PasswordSalt = passwordSalt;

                _dbContext.Usuario.Add(usuario);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(String.IsNullOrEmpty(ex.Message) ? "Ocurrio un problema" : ex.Message);
            }
        }

        public async Task Update(UsuarioDto usuarioDto)
        {
            try
            {
                var usuario = await _dbContext.Usuario.Where(x => x.Id == usuarioDto.Id).FirstOrDefaultAsync();

                if (usuario == null)
                    throw new Exception("El usuario que desea actualizar no existe");

                var emailExist = await _dbContext.Usuario.Where(x => x.Email == usuarioDto.Email && x.Id != usuarioDto.Id).AnyAsync();

                if (emailExist)
                    throw new Exception("El email con el que se intenta actualizar el usuario ya se encuentra registrado");

                usuario.Email = usuarioDto.Email;
                usuario.Activo = usuarioDto.Activo;
                usuario.RolId = usuarioDto.RolId;

                if (!string.IsNullOrEmpty(usuarioDto.PasswordString))
                {
                    CreatePasswordHash(usuarioDto.PasswordString, out byte[] passwordHash, out byte[] passwordSalt);

                    usuario.Password = passwordHash;
                    usuario.PasswordSalt = passwordSalt;
                }

                _dbContext.Entry(usuario).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(String.IsNullOrEmpty(ex.Message) ? "Ocurrio un problema" : ex.Message);
            }
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordDb, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordDb);
            }
        }

    }
}
