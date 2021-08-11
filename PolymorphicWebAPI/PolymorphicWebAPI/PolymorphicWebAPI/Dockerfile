#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443



FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["PolymorphicWebAPI/PolymorphicWebAPI.csproj", "PolymorphicWebAPI/"]
COPY ["PolymorphicWebAPI.Infrastructure/PolymorphicWebAPI.Infrastructure.csproj", "PolymorphicWebAPI.Infrastructure/"]
COPY ["PolymorphicWebAPI.Service/PolymorphicWebAPI.Service.csproj", "PolymorphicWebAPI.Service/"]
COPY ["PolymorphicWebAPI.Persistence/PolymorphicWebAPI.Persistence.csproj", "PolymorphicWebAPI.Persistence/"]
COPY ["PolymorphicWebAPI.Domain/PolymorphicWebAPI.Domain.csproj", "PolymorphicWebAPI.Domain/"]
COPY ["PolymorphicWebAPI.Test/PolymorphicWebAPI.Test.csproj", "PolymorphicWebAPI.Test/"]
RUN dotnet restore "PolymorphicWebAPI/PolymorphicWebAPI.csproj"
COPY . .
WORKDIR "/src/PolymorphicWebAPI"
RUN dotnet build  -c Release -o /app/build

FROM build AS publish
RUN dotnet publish  -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PolymorphicWebAPI.dll"]
