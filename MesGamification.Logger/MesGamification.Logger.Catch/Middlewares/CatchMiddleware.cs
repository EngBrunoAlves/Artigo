using MesGamification.Logger.Catch.Entities;
using MesGamification.Logger.Communicator.Entites;
using MesGamification.Logger.Communicator.Interfaces;
using MesGamification.Logger.Communicator.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MesGamification.Logger.Catch.Middlewares
{
    public class CatchMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CatchMiddleware> _logger;
        private readonly ICommunication _communication;
        private readonly CloudUrls _cloudUrls;

        public CatchMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, ICommunication communication, IOptions<CloudUrls> options)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<CatchMiddleware>();
            _communication = communication ?? throw new ArgumentException(nameof(communication));
            _cloudUrls = options?.Value ?? throw new ArgumentException(nameof(_cloudUrls));
        }

        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;
            var catchDto = new CatchDto
            {
                EventTime = DateTime.UtcNow,
                Method = request.Method,
                PathBase = request.PathBase
            };

            request.EnableRewind();
            using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
            {
                catchDto.Body = reader.ReadToEnd();
            }
            request.Body.Position = 0;

            var log = new UserLog
            {
                UserId = Guid.NewGuid(),
                Body = catchDto.Serialize()
            };

            #pragma warning disable CS4014
            Task.Run(() => { ReportInfo(log); });
            #pragma warning restore CS4014

            await _next(context);
        }

        private void ReportInfo(UserLog log)
        {
            try
            {
                var url = $"{_cloudUrls.MesLogger}/UserLog";
                var result = _communication.PostAsync<object>(url, log);
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Error send log " + ex.Message);
            }
        }
    }
}