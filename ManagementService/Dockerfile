FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY Contracts/Contract/Contract/Contract.csproj Contracts/Contract/Contract/

COPY ManagementService/API/API.csproj API/
COPY ManagementService/Application/Application.csproj Application/
COPY ManagementService/Domain/Domain.csproj Domain/
COPY ManagementService/Infrastructure/Infrastructure.csproj Infrastructure/

RUN dotnet restore API/API.csproj

COPY . .

WORKDIR /src/ManagementService/API
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "API.dll"]