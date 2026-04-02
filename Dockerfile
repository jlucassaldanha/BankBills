FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["BankBills.csproj", "./"]
RUN dotnet restore "BankBills.csproj"

COPY . .
RUN dotnet publish "BankBills.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8000

COPY --from=build /app/publish .

ENTRYPOINT [ "dotnet", "BankBills.dll" ]