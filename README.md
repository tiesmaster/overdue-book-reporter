# overdue-book-reporter

Simple tool to check any outstanding books at the library, and report if any are overdue

# Usage

## CLI

Pass in the required parameters like this:

Via CLI:

```bash
dotnet run --librarylogincredentials:username AzureDiamond --librarylogincredentials:password hunter2
```

Via ENV:

```bash
LIBRARYLOGINCREDENTIALS__USERNAME=AzureDiamond LIBRARYLOGINCREDENTIALS__PASSWORD=hunter2 dotnet run
```

Or set as user secret:

```bash
dotnet user-secrets set 'LibraryLoginCredentials:Username' 'AzureDiamond'
dotnet user-secrets set 'LibraryLoginCredentials:Password' 'hunter2'

# And then just do dotnet run
dotnet run
```

## Docker

```bash
docker run --rm ghcr.io/tiesmaster/overdue-book-reporter:main -e LIBRARYLOGINCREDENTIALS__USERNAME=AzureDiamond -e LIBRARYLOGINCREDENTIALS__PASSWORD=hunter2
```
