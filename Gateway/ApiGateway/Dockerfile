FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["MyECommerce.sln", "./"]
COPY ["Directory.Packages.props", "./"]
COPY ["BuildingBlocks/Shared.API/Shared.API.csproj", "BuildingBlocks/Shared.API/"]
COPY ["Gateway/ApiGateway/ApiGateway.csproj", "Gateway/ApiGateway/"]

RUN dotnet restore "Gateway/ApiGateway/ApiGateway.csproj"
COPY . .
WORKDIR "/src/Gateway/ApiGateway"
RUN dotnet build "ApiGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApiGateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# ACI không hỗ trợ port mapping (không có -p 80:8080 như Docker Desktop).
# Phải set cho .NET lắng nghe đúng port mà ACI mở ra bên ngoài.
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "ApiGateway.dll"]