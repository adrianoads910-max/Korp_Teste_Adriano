# -------------------------
# ✅ Build da aplicação
# -------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia tudo
COPY . .

# Restaura dependências e compila
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# -------------------------
# ✅ Runtime final (imagem pequena)
# -------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

# Porta exposta (Railway detecta automaticamente)
EXPOSE 8080

# Força o ASP.NET a escutar no Railway
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "EstoqueService.dll"]
