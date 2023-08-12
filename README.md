# overdue-book-reporter

Simple tool to check any outstanding books at the library, and report if any are overdue

# Usage

## Via `.env` file

1. Use the [`.env.sample`](./.env.sample) as template to create a new `.env` file
2. Set the appropriate environment variables

## Docker

### Pull
```bash
docker pull ghcr.io/tiesmaster/overdue-book-reporter:main
```

### Run
```bash
docker run --rm ghcr.io/tiesmaster/overdue-book-reporter:main -e LIBRARYLOGINCREDENTIALS__USERNAME=AzureDiamond -e LIBRARYLOGINCREDENTIALS__PASSWORD=hunter2
```

## CLI

:exclamation: This is outdated, rather use the [`.env` file instead](#via-env-file)

Pass in the required parameters like this:

Via CLI:

```bash
dotnet run --project src/OverdueBookReporter --librarylogincredentials:username AzureDiamond --librarylogincredentials:password hunter2
```

Via ENV:

```bash
LIBRARYLOGINCREDENTIALS__USERNAME=AzureDiamond LIBRARYLOGINCREDENTIALS__PASSWORD=hunter2 dotnet run --project src/OverdueBookReporter
```

Or set as user secret:

```bash
dotnet user-secrets set 'LibraryLoginCredentials:Username' 'AzureDiamond'
dotnet user-secrets set 'LibraryLoginCredentials:Password' 'hunter2'

# And then just do dotnet run
dotnet run
```