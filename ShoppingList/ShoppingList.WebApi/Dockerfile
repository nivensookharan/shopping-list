#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ShoppingList.WebApi/ShoppingList.WebApi.csproj", "ShoppingList.WebApi/"]
RUN dotnet restore "./ShoppingList.WebApi/./ShoppingList.WebApi.csproj"
COPY . .
WORKDIR "/src/ShoppingList.WebApi"
RUN dotnet build "./ShoppingList.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ShoppingList.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShoppingList.WebApi.dll"]