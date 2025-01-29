
using GrpcClientApi.Hubs;
using ServerStream;

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

            builder.Services.AddGrpcClient<MessengerServerStream.MessengerServerStreamClient>(o =>
            {
                o.Address = new Uri("http://localhost:5129");
            });

            builder.Services.AddSignalR();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //builder.Services.AddCors(options =>
            //{
            //    options.AddPolicy(name: "CORSPolicy",
            //        builder =>
            //        {
            //            builder.
            //             AllowAnyMethod()
            //            .AllowAnyHeader()
            //            .AllowAnyOrigin();

            //        });
            //});

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("ClientPermission", policy =>
                {
                    policy.AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithOrigins("http://localhost:9000", "https://localhost:9001")
                        .AllowCredentials();
                });
            });

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

            // Add CORS middleware before MVC
            app.UseCors("ClientPermission");

            

            app.MapHub<RealTimeHub>("/realtime");   // ChatHub будет обрабатывать запросы по пути /real

            app.MapControllers();

            app.Run();
        }
    }
}
