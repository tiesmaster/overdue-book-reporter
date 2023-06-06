using Microsoft.Extensions.Configuration;
using Tiesmaster.OverdueBookReporter;

var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .Build();

var credentials = new LibraryLoginCredentials();
config.Bind("LibraryLoginCredentials", credentials);

var client = new LibraryRotterdamClient(credentials);

Console.WriteLine("Starting session");
await client.StartSessionAsync();

Console.WriteLine("Logging in");
await client.LoginAsync();

Console.WriteLine("Retrieving book listing");
var bookListing = await client.GetBookListingAsync();

foreach (var bookTitle in bookListing)
{
    Console.WriteLine(bookTitle);
}