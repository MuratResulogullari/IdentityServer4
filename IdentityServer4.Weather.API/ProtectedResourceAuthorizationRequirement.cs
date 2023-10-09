using IdentityModel.Client;
using IdentityServer4.Weather.API.Models;
using IdentityServer4.Weather.API.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Security.Authentication;
using System.Text.Json;

public class ProtectedResourceAuthorizationRequirement : IAuthorizationRequirement
{
    public string ServiceName { get; }

    public ProtectedResourceAuthorizationRequirement(string serviceName)
    {
        this.ServiceName = serviceName;
    }
}

public class ProtectedResourcePermissionHandler : AuthorizationHandler<ProtectedResourceAuthorizationRequirement>
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _client;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserService _applicationUserService;

    public ProtectedResourcePermissionHandler(IHttpClientFactory httpClientFactory
        , HttpClient client
        , IHttpContextAccessor httpContextAccessor
        , IUserService applicationUserService
        )
    {
        _httpClientFactory = httpClientFactory;
        _client = client;
        _httpContextAccessor = httpContextAccessor;
        _applicationUserService = applicationUserService;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ProtectedResourceAuthorizationRequirement requirement)
    {
        UserInfo userInfo = await _applicationUserService.GetUserInfoAsync();

        var endpoint = _httpContextAccessor.HttpContext.GetEndpoint();
        var descriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();

        string requestControllerName = descriptor.ControllerTypeInfo.Name;
        string requestActionName = _httpContextAccessor.HttpContext.Request.RouteValues["action"].ToString();

        List<ClaimDto> claims = await GetAsync<List<ClaimDto>>();

        if (claims != null)
        {
            var authorizedClaimExist = claims.FirstOrDefault(x => x.Type == $"{requirement.ServiceName}.{requestControllerName}");

            if (authorizedClaimExist != null)
            {
                var permittedRoleClaim = JsonSerializer.Deserialize<RoleClaimValueDto>(authorizedClaimExist.Value);

                // tüm controllerlara yetkili
                if (permittedRoleClaim.SelectionType == RoleClaimTypes.SelectionAll)
                {
                    context.Succeed(requirement);
                } // Sadece bazı actionlara yetki
                else if (permittedRoleClaim.SelectionType == RoleClaimTypes.SelectionPartially)
                {
                    if (permittedRoleClaim.PermittedValues.Contains(requestActionName))
                    {
                        context.Succeed(requirement);
                    }
                    else
                    {
                        context.Fail();
                    }
                }
            }
            else
            {
                context.Fail();
            }
        }
        else
        {
            context.Fail();
        }
    }

    private async Task<T> GetAsync<T>()
    {
        AuthenticateResult? authResult = null;

        HttpClient client = new HttpClient();

        var uri = "https://localhost:5022/api/users/getuserprofile";

        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext != null)
        {
            authResult = await httpContext.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
        }
        else
        {
            throw new AuthenticationException();
        }

        if (authResult?.Succeeded == true)
        {
            var accessToken = await httpContext.GetTokenAsync(scheme: JwtBearerDefaults.AuthenticationScheme, tokenName: OpenIdConnectParameterNames.AccessToken);

            if (!string.IsNullOrEmpty(accessToken))
            {
                _client.SetBearerToken(accessToken);
            }
        }
        else
        {
            throw new AuthenticationException();
        }

        var response = await client.GetAsync(uri);
        if (!response.IsSuccessStatusCode)
        {
            return default(T)!;
        }

        var content = await response.Content.ReadAsStringAsync();

        return JsonExtension.DeserializeJson<T>(content);
    }
}