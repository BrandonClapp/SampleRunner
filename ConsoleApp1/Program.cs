using System;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp1.Migrations;
using ConsoleApp1.Samples;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;

using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // What if I could encapsulate each of these method bodies in their own
            // abstract implementation and then decorate each of the sample classes in order
            // to crawl them with reflection and individually run each of them
            // and then report their status to my console whether they pass or fail
            // depending on their return status or whether they throw exceptions.
            // Should methods be ran in series or parallel?
            // If series, should they have a sequence?
            // Should each method contain it's own scope? Do we store state at the class level? Are the classes singleton or transient?
            
            // App idea:
            //      "Test runner" like application that I can run all methods in a given project
            //      and report their status and return values into a nicely formatted console.

            try
            {
                await Runner.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
            
            return;

            return;
            var serviceProvider = CreateServices();

            // Put the database update into a scope to ensure
            // that all resources will be disposed.
            using (var scope = serviceProvider.CreateScope())
            {
                UpdateDatabase(scope.ServiceProvider);
            }
        }

        /// <summary>
        /// Configure the dependency injection services
        /// </summary>
        private static IServiceProvider CreateServices()
        {
            return new ServiceCollection()
                // Add common FluentMigrator services
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddPostgres()
                    // Set the connection string
                    .WithGlobalConnectionString("User ID=vjdoyenjkwdoja;Password=7b9c2135c8ba53e74dd8ecc9972f3e0f88dbb4c0a0cb8d59f23bfcb3eb714369;Host=ec2-54-157-100-65.compute-1.amazonaws.com;Port=5432;Database=d2445cg3bg1bom;SslMode=Prefer;Trust Server Certificate=true")
                    // Define the assembly containing the migrations
                    .ScanIn(typeof(AddUserTable).Assembly).For.Migrations())
                // Enable logging to console in the FluentMigrator way
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                // Build the service provider
                .BuildServiceProvider(false);
        }

        /// <summary>
        /// Update the database
        /// </summary>
        private static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            // Instantiate the runner
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            // Execute the migrations
            // runner.MigrateUp();
            
            runner.MigrateDown(0);
        }
    }
}