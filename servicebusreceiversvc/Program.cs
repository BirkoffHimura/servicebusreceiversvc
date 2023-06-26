// Program.cs
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

class Program
{
    static void Main(string[] args)
    {
        // Setup Host
        var host = CreateDefaultBuilder().Build();

        // Invoke Worker
        using IServiceScope serviceScope = host.Services.CreateScope();
        IServiceProvider provider = serviceScope.ServiceProvider;
        var workerInstance = provider.GetRequiredService<Worker>();
        workerInstance.DoWork();

        host.Run();
    }

    static IHostBuilder CreateDefaultBuilder()
    {
        return Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(app =>
            {
                app.AddJsonFile("appsettings.json");
            })
            .ConfigureServices(services =>
            {
                services.AddSingleton<Worker>();
            });
    }
}

// Worker.cs
internal class Worker
{
    private readonly IConfiguration configuration;

    public Worker(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public void DoWork()
    {
        var keyValuePairs = configuration.AsEnumerable().ToList();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("==============================================");
        Console.WriteLine("Configurations...");
        Console.WriteLine("==============================================");
        foreach (var pair in keyValuePairs)
        {
            Console.WriteLine($"{pair.Key} - {pair.Value}");
        }
        Console.WriteLine("==============================================");
        Console.ResetColor();
    }
}