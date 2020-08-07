using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Text;
using Core.Extensions;
using Microsoft.AspNetCore.Http.Features;

namespace Business.Autofac
{
    public class SecuredClaimer
    {
        private static IHttpContextAccessor _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();

        public static string GetUserId()
        {
            var claims = _httpContextAccessor.HttpContext.User.Claims("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            return claims?.ToArray()[0];
        }

        public static string GetUsername()
        {
            var claims = _httpContextAccessor.HttpContext.User.Claims("unique_name");
            return claims?.ToArray()[0];
        }

        public static string GetIpAddress()
        {
            return _httpContextAccessor.HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();
        }
    }
}
