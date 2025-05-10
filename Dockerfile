# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["NoteKeeperPro.Web/NoteKeeperPro.Web.csproj", "NoteKeeperPro.Web/"]
COPY ["NoteKeeperPro.Application/NoteKeeperPro.Application.csproj", "NoteKeeperPro.Application/"]
COPY ["NoteKeeperPro.Domain/NoteKeeperPro.Domain.csproj", "NoteKeeperPro.Domain/"]
COPY ["NoteKeeperPro.Infrastructure/NoteKeeperPro.Infrastructure.csproj", "NoteKeeperPro.Infrastructure/"]
RUN dotnet restore "NoteKeeperPro.Web/NoteKeeperPro.Web.csproj"

# Copy the rest of the code
COPY . .

# Build and publish
RUN dotnet build "NoteKeeperPro.Web/NoteKeeperPro.Web.csproj" -c Release -o /app/build
RUN dotnet publish "NoteKeeperPro.Web/NoteKeeperPro.Web.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "NoteKeeperPro.Web.dll"] 