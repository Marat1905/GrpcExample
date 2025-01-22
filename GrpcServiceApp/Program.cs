using GrpcServiceApp.Services;

namespace GrpcServiceApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // добавляем сервисы для работы с gRPC
            builder.Services.AddGrpc();

            var app = builder.Build();

            // встраиваем TranslatorService в обработку запроса
            app.MapGrpcService<TranslatorService>();

            // встраиваем InviterService в обработку запроса
            app.MapGrpcService<InviterService>();

            // встраиваем MessengerServerStreamService в обработку запроса
            app.MapGrpcService<MessengerServerStreamService>();

            app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}
