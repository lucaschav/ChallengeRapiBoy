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
    public class RolService : BaseService, IRolService
    {
        private readonly IHttpClientFactory _httpClient;

        public RolService(IHttpClientFactory httpClient) : base(httpClient)
        {
            this._httpClient = httpClient;
        }


        public async Task<IEnumerable<RolDto>> GetRoles(string token)
        {
            var response = await SendAsync<IEnumerable<RolDto>>(new ApiRequest
            {
                ApiType = SD.ApiType.GET,
                Url = SD.UrlApiBase + "Rol/GetAll",
                Authorize = true,
                AccessToken =  token
            });
            return response;
        }
    }
}
