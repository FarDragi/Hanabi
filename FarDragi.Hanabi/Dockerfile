FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FarDragi.Hanabi/FarDragi.Hanabi.csproj", "FarDragi.Hanabi/"]

RUN dotnet nuget add source --store-password-in-clear-text "https://nuget.pkg.github.com/Takasaki-Studio/index.json" --name github --username ${GITHUB_NAME} --password ${GITHUB_TOKEN}

RUN dotnet restore "FarDragi.Hanabi/FarDragi.Hanabi.csproj"
COPY . .
WORKDIR "/src/FarDragi.Hanabi"
RUN dotnet build "FarDragi.Hanabi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FarDragi.Hanabi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FarDragi.Hanabi.dll"]
