
using GrpcServerApp.Context;
using System.Diagnostics;
using System.Threading;

namespace GrpcServerApp
{
    public class MyHostedService : BackgroundService
    {
        private readonly ILogger<MyHostedService> _logger;
        private readonly HostModel _hostModel;
        private readonly ApplicationContext _db;

        public MyHostedService(ILogger<MyHostedService> logger, HostModel hostModel, ApplicationContext db)
        {
            _logger = logger;
            _hostModel = hostModel;
            _db = db;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
            // Выполняем задачу пока не будет запрошена остановка приложения
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    Debug.WriteLine($"Запись {DateTime.Now}");
                    _hostModel.Text = $"Запись {DateTime.Now}";
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
