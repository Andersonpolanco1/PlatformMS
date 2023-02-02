using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

namespace CommandService.Data
{
    public class AutoMigrate
    {
        public static void CommandsDbPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<ApplicationDbContext>());
            }
        }

        private static void SeedData(ApplicationDbContext context)
        {
            try
            {
                if(context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                    Console.WriteLine($"--> Successfully migrated!");
                }
                else
                {
                    Console.WriteLine($"--> No pending migrations!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not migrate: {ex.Message}");
            }

            // Seed data here
            
        }
    }
}
