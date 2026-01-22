FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src
COPY . .
RUN dotnet publish "eBoardAPI.csproj" -o /published /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS runtime
EXPOSE 8080
WORKDIR /app
COPY --from=build /published .
ENTRYPOINT [ "dotnet", "eBoardAPI.dll" ]