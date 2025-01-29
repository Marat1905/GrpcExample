
using Grpc.Core;
using GrpcServerApp.Context;
using GrpcServerApp.Services;
using Microsoft.AspNetCore.Hosting.Server;
using ServerStream;
using System.Diagnostics;
using System.Threading;

namespace GrpcServerApp
{
    public class MyHostedService : BackgroundService
    {
        private readonly ILogger<MyHostedService> _logger;
        private readonly HostModel _hostModel;
        private readonly ApplicationContext _db;
        private readonly StreamingServer _server;
        private IServerStreamWriter<ResponseServerStream>?  _serverStreamingCall;

        public MyHostedService(ILogger<MyHostedService> logger, HostModel hostModel, ApplicationContext db, StreamingServer server)
        {
            _logger = logger;
            _hostModel = hostModel;
            _db = db;
            _server = server;
           
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Выполняем задачу пока не будет запрошена остановка приложения
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _serverStreamingCall = (IServerStreamWriter<ResponseServerStream>?)_server;
                    Debug.WriteLine($"Запись {DateTime.Now}");
                    _serverStreamingCall.WriteAsync(new ResponseServerStream() { Content = $"Запись {DateTime.Now}" });
                    //_hostModel.Text = $"Запись {DateTime.Now}";
                }
                catch (Exception ex)
                {
                    // обработка ошибки однократного неуспешного выполнения фоновой задачи
                }

                await Task.Delay(5000);
            }

        }
    }
}
