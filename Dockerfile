FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["dotnet-rpg.csproj", "./"]
RUN dotnet restore "dotnet-rpg.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "dotnet-rpg.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "dotnet-rpg.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "dotnet-rpg.dll"]
