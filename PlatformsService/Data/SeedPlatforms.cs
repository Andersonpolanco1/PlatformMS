using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PlatformService.Models;
using System;
using System.Linq;

namespace PlatformService.Data
{
    public class SeedPlatforms
    { 
        public static void PlatformDbPopulation(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using(var serviceScope = app.ApplicationServices.CreateScope())
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
            }

            if (!context.Platforms.Any())
            {
                context.AddRange(
                     new Platform { Name = "Microsoft", Cost = "Free", Publishser = "Microsoft" },
                     new Platform { Name = "Kubernetes", Cost = "Free", Publishser = "Microsoft SQL" },
                     new Platform { Name = "Docker", Cost = "Free", Publishser = "Microsoft" }
                 );
                context.SaveChanges();
                Console.WriteLine("--> Seeded");
            }
        }
    }
}
