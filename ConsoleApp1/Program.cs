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
            try
            {
                await Runner.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
        }
    }
}