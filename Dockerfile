FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /build

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish src/OverdueBookReporter -c Release -o /build

# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime:7.0
WORKDIR /app
COPY --from=base /build .
ENTRYPOINT ["dotnet", "OverdueBookReporter.dll"]