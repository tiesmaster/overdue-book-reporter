# overdue-book-reporter

Simple tool to check any outstanding books at the library, and report if any are overdue

# Usage

## Helm Chart

Add this repo as 'helm repo', and install using `overdue-book-reporter/overdue-book-reporter`:

```
helm repo add overdue-book-reporter https://raw.githubusercontent.com/tiesmaster/overdue-book-reporter/main/charts
helm install john overdue-book-reporter/overdue-book-reporter
```

These [values](./charts/overdue-book-reporter/values.yaml) are available.

## Via `.env` file

1. Use the [`.env.sample`](./.env.sample) as template to create a new `.env` file
2. Set the appropriate environment variables


## Enabling outgoing HTTP requests logging middleware

Set this environment variable to enable the [outgoing request middleware](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-7.0#outgoing-request-middleware):

```
Logging__LogLevel__System.Net.Http.HttpClient.LibraryRotterdamClient=Trace
```

### Development

_For during development, make use of the [`.env_`](./.env) file._


### Enable this in Kubernetes

Modify the `cronjob.yaml` deployment to add the environment variable to the Pod `spec:`

```
env:
- name: Logging__LogLevel__System.Net.Http.HttpClient.LibraryRotterdamClient
  value: Trace
```

Or via `kubectl`, like this: create a new job, based on the cronjob, and add environment variables using `jq` (resource: [SO](https://stackoverflow.com/a/65140499/471780)):

```
kubectl create job --from=cj/overdue-book-reporter-jurre --dry-run=client manual-jurre-run -o json \
| jq ".spec.template.spec.containers[0].env += [{ \"name\": \"Logging__LogLevel__System.Net.Http.HttpClient.LibraryRotterdamClient\", value:\"Trace\" }]" \
| kubectl apply -f -
```


## Docker

### Pull
```bash
docker pull ghcr.io/tiesmaster/overdue-book-reporter
```

### Run
```bash
docker run --rm \
    -e LIBRARYROTTERDAMCLIENT__USERNAME=AzureDiamond \
    -e LIBRARYROTTERDAMCLIENT__PASSWORD=hunter2 \
    -e EMAILSETTINGS__FROM__NAME=OverdueBookReporter \
    -e EMAILSETTINGS__FROM__ADDRESS=john@gmail.com \
    -e EMAILSETTINGS__TO__NAME=John \
    -e EMAILSETTINGS__TO__ADDRESS=john@gmail.com \
    -e EMAILSETTINGS__MAILSERVER__HOST=smtp.gmail.com \
    -e EMAILSETTINGS__MAILSERVER__PORT=465 \
    -e EMAILSETTINGS__MAILSERVER__USESSL=true \
    -e EMAILSETTINGS__MAILSERVER__USERNAME=john@gmail.com \
    -e EMAILSETTINGS__MAILSERVER__PASSWORD=hunter2 \
    ghcr.io/tiesmaster/overdue-book-reporter
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
LIBRARYROTTERDAMCLIENT__USERNAME=AzureDiamond LIBRARYROTTERDAMCLIENT__PASSWORD=hunter2 dotnet run --project src/OverdueBookReporter
```

Or set as user secret:

```bash
dotnet user-secrets set 'LibraryRotterdamClient:Username' 'AzureDiamond'
dotnet user-secrets set 'LibraryRotterdamClient:Password' 'hunter2'

# And then just do dotnet run
dotnet run
```