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
            app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}
