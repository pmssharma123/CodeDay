# ---------- Build Stage ----------
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy the solution and restore dependencies
COPY CoDayContest.sln ./
COPY CoDayContest/*.csproj ./CoDayContest/
RUN dotnet restore CoDayContest.sln

# Copy the full source code and build
COPY . ./
RUN dotnet publish CoDayContest/CoDayContest.csproj -c Release -o /app/publish

# ---------- Runtime Stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "CoDayContest.dll"]
