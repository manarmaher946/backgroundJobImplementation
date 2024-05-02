using backgroundImplementation.BackgroundService;
using backgroundImplementation.cashing_services;
using backgroundImplementation.Data;
using backgroundImplementation.DistributedChashing;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
namespace backgroundImplementation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            IConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:6379");
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ApplicationDbcontext>(options =>
           options.UseSqlServer(builder.Configuration.GetConnectionString("default"))
           );
            builder.Services.AddTransient<IsendService, SendService>();
            builder.Services.AddScoped<Icashing_service, cashing_service>();
            builder.Services.AddScoped<IDistributed,Distributed>();


            builder.Services.AddHangfire(c => c
            .UseMemoryStorage());
            
            builder.Services.AddHangfireServer();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseHangfireDashboard();
            app.MapControllers();
            RecurringJob.AddOrUpdate<IsendService>(x=>x.updateDatabase(), "* * * ? * *");
            app.Run();
        }
    }
}
