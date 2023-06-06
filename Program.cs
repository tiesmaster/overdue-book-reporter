using Tiesmaster.OverdueBookReporter;

var client = new LibraryRotterdamClient(new(args[0], args[1]));

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