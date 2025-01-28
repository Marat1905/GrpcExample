
using GrpcClientApi.Hubs;

namespace GrpcClientApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddGrpcClient<UserService.UserServiceClient>(o =>
            {
                o.Address = new Uri("http://localhost:5129");
            });

            builder.Services.AddSignalR();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                //app.UseSwaggerUI(o =>
                //{
                //    o.InjectStylesheet("/css/swagger-custom.css");
                //});
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.MapHub<RealTimeHub>("/real");   // ChatHub будет обрабатывать запросы по пути /real

            app.MapControllers();

            app.Run();
        }
    }
}
