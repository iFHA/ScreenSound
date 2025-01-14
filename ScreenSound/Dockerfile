# Estágio de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copiar csproj e restaurar dependências
COPY *.csproj ./
RUN dotnet restore

# Copiar todo o código e buildar a aplicação
COPY . ./
RUN dotnet publish -c Release -o /app/out

# Estágio final
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copiar o build do estágio anterior
COPY --from=build-env /app/out .

# Expor a porta necessária
EXPOSE 80

# Definir o ponto de entrada
ENTRYPOINT ["dotnet", "ScreenSound.dll"]
