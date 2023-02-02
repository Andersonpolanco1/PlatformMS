using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace CommandService.Data
{
    public class AutoMigrate
    {
        public static void CommandsDbPopulation(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<ApplicationDbContext>(), env);
            }
        }

        private static void SeedData(ApplicationDbContext context, IWebHostEnvironment env)
        {
            if (env.IsProduction())
            {
                try
                {
                    context.Database.Migrate();
                    Console.WriteLine($"--> Successfully migrated!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Could not migrate. {ex.Message}");
                }

                // Seed data here
            }
        }
    }
}
