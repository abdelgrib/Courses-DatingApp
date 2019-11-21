using System;
using DatingApp.API.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DatingApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build(); 
            /*Seed (create/migrate) database*/
            using(var scope = host.Services.CreateScope()) /*Can't use injection, use services*/
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<DataContext>();
                    context.Database.Migrate(); //"dontnet ef database migrate" to create/apply pending migrations if necessary
                    Seed.SeedUsers(context);
                }
                catch(Exception exception)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(exception, $"Error : {nameof(Seed)}");
                } 
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder => 
            {
                webBuilder.UseStartup<Startup>();
            });
                
    }
}
