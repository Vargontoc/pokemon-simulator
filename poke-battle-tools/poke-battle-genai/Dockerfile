FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY ./ ./
RUN dotnet publish ./ -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_URLS=http://+:6000
EXPOSE 6000

ENTRYPOINT ["dotnet", "poke-battle-genai.dll"]