using GrpcServiceApp.Services;

namespace GrpcServiceApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ��������� ������� ��� ������ � gRPC
            builder.Services.AddGrpc();

            var app = builder.Build();

            // ���������� TranslatorService � ��������� �������
            app.MapGrpcService<TranslatorService>();

            // ���������� InviterService � ��������� �������
            app.MapGrpcService<InviterService>();

            // ���������� MessengerServerStreamService � ��������� �������
            app.MapGrpcService<MessengerServerStreamService>();

            app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}
