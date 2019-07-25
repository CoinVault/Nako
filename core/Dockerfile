FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /app
COPY *.csproj ./
COPY . ./
RUN dotnet restore 

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
LABEL Author="CoinVault"
LABEL Maintainer="Dan Gershony"
COPY --from=publish /app .
EXPOSE 9000
ENTRYPOINT ["dotnet", "Nako.dll"]