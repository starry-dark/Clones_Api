#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY *.sln .

COPY ["Clones_Api/Clones_Api.csproj", "Clones_Api/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Data/Data.csproj", "Data/"]
COPY ["Models/Models.csproj", "Models/"]

RUN dotnet restore "Clones_Api/Clones_Api.csproj"
COPY . .

WORKDIR /src/Clones_Api
RUN dotnet build

FROM build AS publish
WORKDIR /src/Clones_Api
RUN dotnet publish -c Release -o /src/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /src/publish .

#ENTRYPOINT ["dotnet", "Clones_Api.dll"]
CMD ASPNETCORE_URLS=http://*:$PORT dotnet Clones_Api.dll