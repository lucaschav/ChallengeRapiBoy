using Challenge.shared.RequestModels;
using Challenge.web.Models;
using Challenge.web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.shared.Dtos;
using System.Net.Http;
using System.Security.Claims;

namespace Challenge.web.Services
{
    public class UsuarioService : BaseService, IUsuarioService
    {
        private readonly IHttpClientFactory _httpClient;

        public UsuarioService(IHttpClientFactory httpClient) : base(httpClient)
        {
            this._httpClient = httpClient;
        }

        public async Task<bool> Delete(int usuarioId, string token)
        {
            try
            {
                var response = await SendAsync<bool>(new ApiRequest
                {
                    ApiType = SD.ApiType.DELETE,
                    Url = SD.UrlApiBase + "Auth/Delete/" + usuarioId,
                    Authorize = true,
                    AccessToken = token
                });
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<UsuarioDto>> GetUsuarios(string token)
        {
            var response = await SendAsync<IEnumerable<UsuarioDto>>(new ApiRequest
            {
                ApiType = SD.ApiType.GET,
                Url = SD.UrlApiBase + "Auth/GetAll",
                Authorize = true,
                AccessToken =  token
            });
            return response;
        }

        public async Task<List<Claim>> Login(LoginRequest loginRequest)
        {
            try
            {
                var response = await SendAsync<UsuarioDto>(new ApiRequest
                {
                    ApiType = SD.ApiType.POST,
                    Url = SD.UrlApiBase + "Auth/Login",
                    Data = loginRequest,
                    Authorize = false
                });
                    return new List<Claim>
                {
                    new Claim(ClaimTypes.Name,response.Email),
                    new Claim("usuarioId",response.Id.ToString()),
                    new Claim("token", response.Token),
                    new Claim(ClaimTypes.Role, response.Rol.Nombre),
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Register(RegisterRequest usuarioDto)
        {
            try
            {
                var response = await SendAsync<bool>(new ApiRequest
                {
                    ApiType = SD.ApiType.POST,
                    Url = SD.UrlApiBase + "Auth/Register",
                    Data = usuarioDto,
                    Authorize = false
                });

                return response;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Update(UsuarioDto usuarioDto, string token)
        {
            try
            {
                var response = await SendAsync<bool>(new ApiRequest
                {
                    ApiType = SD.ApiType.PUT,
                    Url = SD.UrlApiBase + "Auth/Update",
                    Data = usuarioDto,
                    Authorize = true,
                    AccessToken = token
                });

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
