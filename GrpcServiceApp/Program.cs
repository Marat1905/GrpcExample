using GrpcServiceApp.Services;
using Microsoft.EntityFrameworkCore;

namespace GrpcServiceApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // строка подключения
            string connStr = "Server=(localdb)\\mssqllocaldb;Database=grpcdb;Trusted_Connection=True;";
            // добавляем контекст ApplicationContext в качестве сервиса в приложение
            builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connStr));

            // добавляем сервисы для работы с gRPC
            builder.Services.AddGrpc();

            var app = builder.Build();

            // встраиваем TranslatorService в обработку запроса
            app.MapGrpcService<TranslatorService>();

            // встраиваем InviterService в обработку запроса
            app.MapGrpcService<InviterService>();

            // встраиваем MessengerServerStreamService в обработку запроса
            app.MapGrpcService<MessengerServerStreamService>();

            // встраиваем MessengerClientStreamService в обработку запроса
            app.MapGrpcService<MessengerClientStreamService>();

            // встраиваем MessengerStreamBothWaysService в обработку запроса
            app.MapGrpcService<MessengerStreamBothWaysService>();

            // встраиваем MessagerHeaderService в обработку запроса
            app.MapGrpcService<MessagerHeaderService>();

            // встраиваем UserApiService в обработку запроса
            app.MapGrpcService<UserApiService>();

            app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}
