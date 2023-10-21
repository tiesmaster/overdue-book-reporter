using dotenv.net;

using System.Reflection;

using Tiesmaster.OverdueBookReporter;

LoadDotEnv();
PrintVersionInfo();

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) => Bootstrapper.Bootstrap(services, context.Configuration))
    .Build();

host.Run();

static void LoadDotEnv()
{
    DotEnv
        .Fluent()
        .WithProbeForEnv(probeLevelsToSearch: 6) // go up all the way to the root of the project
        .Load();
}

static void PrintVersionInfo()
{
    var versionInfo = Assembly.GetEntryAssembly()!.GetCustomAttributes<AssemblyInformationalVersionAttribute>().First();
    Console.WriteLine($"overdue-book-reporter: version {versionInfo.InformationalVersion}");
}