# Use the official .NET SDK base image for ARM64
FROM --platform=linux/arm64 mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Set the working directory in the container
WORKDIR /src

# Restore any dependencies (via `dotnet restore`)
COPY ./Quiz.Master/Quiz.Master.csproj ./Quiz.Master/Quiz.Master.csproj
COPY ./Quiz.Common/Quiz.Common.csproj ./Quiz.Common/Quiz.Common.csproj
COPY ./Quiz.Common.Broker/Quiz.Common.Broker.csproj ./Quiz.Common.Broker/Quiz.Common.Broker.csproj
COPY ./Quiz.Common.Messages/Quiz.Common.Messages.csproj ./Quiz.Common.Messages/Quiz.Common.Messages.csproj
RUN dotnet restore ./Quiz.Master/Quiz.Master.csproj

# Copy all project files into the container's /src directory
COPY ./Quiz.Master/ ./Quiz.Master/
COPY ./Quiz.Common/ ./Quiz.Common/
COPY ./Quiz.Common.Broker/ ./Quiz.Common.Broker/
COPY ./Quiz.Common.Messages/ ./Quiz.Common.Messages/

# Publish the project in release mode for ARM64
RUN dotnet publish \
-r linux-arm64 \
-c Release \
-o /app/dist \
./Quiz.Master/Quiz.Master.csproj

# Define the output directory where the compiled app will be
WORKDIR /app

ENTRYPOINT ["sh", "-c", "cp -r /app/dist/* /app/out/"]
