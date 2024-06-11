FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Ejercicio4Modulo2/Ejercicio4Modulo2.csproj", "Ejercicio4Modulo2/"]
RUN dotnet restore "Ejercicio4Modulo2/Ejercicio4Modulo2.csproj"
COPY . .
WORKDIR "/src/Ejercicio4Modulo2"
RUN dotnet build "Ejercicio4Modulo2.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Ejercicio4Modulo2.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ejercicio4Modulo2.dll"]
