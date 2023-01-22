using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PlatformService.Models;
using System;
using System.Linq;

namespace PlatformService.Data
{
    public class SeedPlatforms
    { 
        public static void PlatformDbPopulation(IApplicationBuilder app)
        {
            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<ApplicationDbContext>());
            }
        }

        private static void SeedData(ApplicationDbContext context)
        {
            if (context.Platforms.Any())
            {
                Console.WriteLine("We already have data");
            }
            else
            {
                Console.WriteLine("Seeding");
                context.AddRange(
                    new Platform { Name = "Microsoft", Cost = "Free", Publishser = "Microsoft" },
                    new Platform { Name = "Kubernetes", Cost = "Free", Publishser = "Microsoft SQL" },
                    new Platform { Name = "Docker", Cost = "Free", Publishser = "Microsoft" }
                );
                context.SaveChanges();
                Console.WriteLine("Seeded");
            }



        }
    }
}
