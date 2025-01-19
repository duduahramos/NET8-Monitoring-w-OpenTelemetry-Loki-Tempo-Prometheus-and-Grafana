FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["API/API/API.csproj", "API/API/"]
COPY ["Core/Core/Core.csproj", "Core/Core/"]
RUN dotnet restore "./API/API/API.csproj"
COPY . .
WORKDIR "/src/API/API"
RUN dotnet build "./API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Copy the startup script
COPY ./API/API/start.sh /app/start.sh
USER root
RUN chmod +x /app/start.sh

# Set the entrypoint
CMD ["/app/start.sh"]
ENTRYPOINT ["dotnet", "API.dll"]