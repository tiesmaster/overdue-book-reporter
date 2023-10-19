using dotenv.net;

using OverdueBookReporter;

using System.Reflection;

DotEnv
    .Fluent()
    .WithProbeForEnv(probeLevelsToSearch: 6) // go up all the way to the root of the project
    .Load();

var versionInfo = Assembly.GetEntryAssembly()!.GetCustomAttributes<AssemblyInformationalVersionAttribute>().First();
Console.WriteLine($"overdue-book-reporter: version {versionInfo.InformationalVersion}");

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<MainUseCase>();
    })
    .Build();

host.Run();

