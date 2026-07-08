# Stage 1: Runtime (Sử dụng image nhẹ để chạy app)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Stage 2: SDK (Dùng để build source code)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy file solution và các file .csproj để restore (tối ưu cache của Docker)
COPY ["MyECommerce.sln", "./"]
COPY ["Directory.Packages.props", "./"]
COPY ["BuildingBlocks/Shared.Infrastructure/Shared.Infrastructure.csproj", "BuildingBlocks/Shared.Infrastructure/"]
COPY ["BuildingBlocks/Shared.Domain/Shared.Domain.csproj", "BuildingBlocks/Shared.Domain/"]
COPY ["BuildingBlocks/Shared.Application/Shared.Application.csproj", "BuildingBlocks/Shared.Application/"]
COPY ["BuildingBlocks/Shared.API/Shared.API.csproj", "BuildingBlocks/Shared.API/"]

COPY ["Services/ProductService/src/ProductService.API/ProductService.API.csproj", "Services/ProductService/src/ProductService.API/"]
COPY ["Services/ProductService/src/ProductService.Application/ProductService.Application.csproj", "Services/ProductService/src/ProductService.Application/"]
COPY ["Services/ProductService/src/ProductService.Domain/ProductService.Domain.csproj", "Services/ProductService/src/ProductService.Domain/"]
COPY ["Services/ProductService/src/ProductService.Infrastructure/ProductService.Infrastructure.csproj", "Services/ProductService/src/ProductService.Infrastructure/"]

# Restore các dependencies
RUN dotnet restore "Services/ProductService/src/ProductService.API/ProductService.API.csproj"
RUN dotnet restore "Services/ProductService/src/ProductService.Application/ProductService.Application.csproj"
RUN dotnet restore "Services/ProductService/src/ProductService.Domain/ProductService.Domain.csproj"
RUN dotnet restore "Services/ProductService/src/ProductService.Infrastructure/ProductService.Infrastructure.csproj"


# Copy toàn bộ source code
COPY . .

# Build dự án
WORKDIR "/src/Services/ProductService/src/ProductService.API"
RUN dotnet build "ProductService.API.csproj" -c Release -o /app/build

# Stage 3: Publish
FROM build AS publish
RUN dotnet publish "ProductService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 4: Final (Copy kết quả đã build vào image runtime)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductService.API.dll"]