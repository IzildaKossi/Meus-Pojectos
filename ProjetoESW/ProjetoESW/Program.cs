using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ESW19Backup2.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ESW19Backup2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);
            /* using (var scope = host.Services.CreateScope())
             {
                 var services = scope.ServiceProvider;
                 try
                 {
                     var context = services.GetRequiredService<ApplicationDbContext>();
                     DbInitializer.Initialize(context);
                 }
                 catch (Exception ex)
                 {
                     var logger = services.GetRequiredService<ILogger<Program>>();
                     logger.LogError(ex, "Um erro ocorreu ao tentar enviar a base de dados");
                 }
             }*/
            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseSetting("detailedErrors", "true")
                .CaptureStartupErrors(true)
                .UseStartup<Startup>()
                .Build();
    }
}
