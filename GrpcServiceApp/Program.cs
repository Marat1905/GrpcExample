using GrpcServiceApp.Services;
using Microsoft.EntityFrameworkCore;

namespace GrpcServiceApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ������ �����������
            string connStr = "Server=(localdb)\\mssqllocaldb;Database=grpcdb;Trusted_Connection=True;";
            // ��������� �������� ApplicationContext � �������� ������� � ����������
            builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connStr));

            // ��������� ������� ��� ������ � gRPC
            builder.Services.AddGrpc();

            var app = builder.Build();

            // ���������� TranslatorService � ��������� �������
            app.MapGrpcService<TranslatorService>();

            // ���������� InviterService � ��������� �������
            app.MapGrpcService<InviterService>();

            // ���������� MessengerServerStreamService � ��������� �������
            app.MapGrpcService<MessengerServerStreamService>();

            // ���������� MessengerClientStreamService � ��������� �������
            app.MapGrpcService<MessengerClientStreamService>();

            // ���������� MessengerStreamBothWaysService � ��������� �������
            app.MapGrpcService<MessengerStreamBothWaysService>();

            // ���������� MessagerHeaderService � ��������� �������
            app.MapGrpcService<MessagerHeaderService>();

            // ���������� UserApiService � ��������� �������
            app.MapGrpcService<UserApiService>();

            app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}
