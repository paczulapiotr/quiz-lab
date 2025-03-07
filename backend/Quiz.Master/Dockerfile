
# Use the official ASP.NET Core runtime as a base image
FROM mcr.microsoft.com/dotnet/aspnet:9.0-noble AS base
EXPOSE 8080

# Use the official ASP.NET Core build image
FROM mcr.microsoft.com/dotnet/sdk:9.0-noble AS publish
ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY . .
# Restore
RUN dotnet restore ./Quiz.Master/Quiz.Master.csproj
# Publish
RUN dotnet publish "./Quiz.Master" -c $BUILD_CONFIGURATION \
-p:PublishSingleFile=false -o /src/publish

# Final stage/image
FROM base AS final
USER app
WORKDIR /src
COPY --from=publish /src/publish .
ENTRYPOINT ["dotnet", "./Quiz.Master.dll"]