# -------------------------
# ✅ Build da aplicação
# -------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .

RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# -------------------------
# ✅ Runtime final
# -------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

# Railway envia o URL do EstoqueService por variável de ambiente
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "FaturamentoService.dll"]
