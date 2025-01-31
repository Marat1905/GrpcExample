using GrpcServerApp.Context;
using GrpcServerApp.Services;
using Microsoft.EntityFrameworkCore;

namespace GrpcServerApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // ������ �����������
            string connStr = "Server=(localdb)\\mssqllocaldb;Database=grpcdb;Trusted_Connection=True;";
            // ��������� �������� ApplicationContext � �������� ������� � ����������
            builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connStr), ServiceLifetime.Singleton);

            // Add services to the container.
            builder.Services.AddGrpc();
            //builder.Services.AddSingleton<StreamingServer>();

            builder.Services.AddSingleton(new HostModel());

            //builder.Services.AddHostedService<MyHostedService>();
            builder.Services.AddHostedService<TimerHostService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.MapGrpcService<UserApiService>();
            // ���������� StreamingServer � ��������� �������
            app.MapGrpcService<StreamingServer>();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.Run();
        }
    }
}