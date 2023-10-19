using OverdueBookReporter;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();

// using dotenv.net;

// using MailKit.Net.Smtp;

// using Microsoft.Extensions.Configuration;

// using MimeKit;

// using System.Reflection;

// using Tiesmaster.OverdueBookReporter;

// DotEnv
//     .Fluent()
//     .WithProbeForEnv(probeLevelsToSearch: 6) // go up all the way to the root of the project
//     .Load();

// var config = new ConfigurationBuilder()
//     .AddUserSecrets<Program>()
//     .AddEnvironmentVariables()
//     .AddCommandLine(args)
//     .Build();

// var versionInfo = Assembly.GetEntryAssembly()!.GetCustomAttributes<AssemblyInformationalVersionAttribute>().First();
// Console.WriteLine($"overdue-book-reporter: version {versionInfo.InformationalVersion}");

// var credentials = new LibraryLoginCredentials();
// config.Bind("LibraryLoginCredentials", credentials);

// var emailSettings = new EmailSettings();
// config.Bind("EmailSettings", emailSettings);

// var client = new LibraryRotterdamClient(credentials);

// Console.WriteLine("Starting session");
// await client.StartSessionAsync();

// Console.WriteLine("Logging in");
// await client.LoginAsync();

// Console.WriteLine("Retrieving book listing");
// var bookListing = await client.GetBookListingAsync();

// foreach (var bookTitle in bookListing)
// {
//     Console.WriteLine(bookTitle);
// }

// var today = DateOnly.FromDateTime(DateTime.Today);

// var anyOverdue = bookListing.Any(x => x.GetStatus(today) == BookLoanStatus.Overdue);

// await SendEmailAsync(anyOverdue, emailSettings);

// static async Task SendEmailAsync(bool anyOverdue, EmailSettings settings)
// {
//     var message = new MimeMessage();
//     message.From.Add(new MailboxAddress(settings.From.Name, settings.From.Address));
//     message.To.Add(new MailboxAddress(settings.To.Name, settings.To.Address));
//     message.Subject = anyOverdue ? "OVERDUE!!!" : "all good!";

//     // message.Body = new TextPart("plain")
//     // {
//     //     Text = @""
//     // };

//     using var smtpClient = new SmtpClient();

//     var mailServerSettings = settings.MailServer;
//     smtpClient.Connect(host: mailServerSettings.Host, port: mailServerSettings.Port, useSsl: mailServerSettings.UseSsl);
//     await smtpClient.AuthenticateAsync(mailServerSettings.Username, mailServerSettings.Password);

//     await smtpClient.SendAsync(message);

//     await smtpClient.DisconnectAsync(quit: true);
// }

// public class EmailSettings
// {
//     public EmailAddress From { get; set; } = null!;
//     public EmailAddress To { get; set; } = null!;
//     public MailServerSettings MailServer { get; set; } = null!;
// }

// public class EmailAddress
// {
//     public string Name { get; set; } = null!;
//     public string Address { get; set; } = null!;
// }

// public class MailServerSettings
// {
//     public string Host { get; set; } = null!;
//     public int Port { get; set; }
//     public bool UseSsl { get; set; }
//     public string Username { get; set; } = null!;
//     public string Password { get; set; } = null!;
// }