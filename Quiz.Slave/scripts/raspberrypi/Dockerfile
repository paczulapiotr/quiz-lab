# Use the official .NET SDK base image for ARM64
FROM --platform=linux/arm64 mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Install the necessary dependencies for AOT compilation and other tools
RUN apt update \
 && apt install -y build-essential \
 && rm -rf /var/lib/apt/lists/*

# Set the working directory in the container
WORKDIR /src

# Restore any dependencies (via `dotnet restore`)
COPY ./Quiz.Slave.csproj ./Quiz.Slave.csproj
RUN dotnet restore

# Copy all project files into the container's /src directory
COPY . .

# Publish the project in release mode for ARM64
RUN dotnet publish \
-r linux-arm64 \
-c Release \
-p:DebugType=None \
-p:DebugSymbols=false \
-o /app/dist \
./Quiz.Slave.csproj

# Define the output directory where the compiled app will be
WORKDIR /app

ENTRYPOINT ["sh", "-c", "cp -r /app/dist/* /app/out/"]
