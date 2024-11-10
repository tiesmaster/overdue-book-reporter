FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG TARGETARCH
WORKDIR /build
# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore -a $TARGETARCH
# Build and publish a release
RUN dotnet publish -a $TARGETARCH src/OverdueBookReporter -c Release -o /build

# Build runtime image
FROM base as final
WORKDIR /app
COPY --from=build /build .
ENTRYPOINT ["dotnet", "OverdueBookReporter.dll"]