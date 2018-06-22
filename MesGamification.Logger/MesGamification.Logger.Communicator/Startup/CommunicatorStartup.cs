using MesGamification.Logger.Communicator.Interfaces;
using MesGamification.Logger.Communicator.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MesGamification.Logger.Communicator.Startup
{
    public static class CommunicatorStartup
    {
        public static void HttpCommunicationInitialize(this IServiceCollection services, IConfiguration Configuration, bool singleton = false)
        {
            if(singleton)
                services.AddSingleton<ICommunication, HttpCommunication>();
            else
                services.AddScoped<ICommunication, HttpCommunication>();
        }
    }
}