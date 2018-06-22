using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MesGamification.Logger.Communicator.Services
{
    public static class ServiceLocator
    {
        public static IServiceProvider Instance { private get; set; }

        public static T Resolve<T>() => Instance.GetService<T>();

        public static T ResolveByContext<T>() => Instance.GetService<IHttpContextAccessor>().HttpContext.RequestServices.GetService<T>();
    }
}