FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["AdventOfCode.2023.1/AdventOfCode.2023.1.csproj", "AdventOfCode.2023.1/"]
RUN dotnet restore "AdventOfCode.2023.1/AdventOfCode.2023.1.csproj"
COPY . .
WORKDIR "/src/AdventOfCode.2023.1"
RUN dotnet build "AdventOfCode.2023.1.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AdventOfCode.2023.1.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AdventOfCode.2023.1.dll"]
