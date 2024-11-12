#!/bin/bash

# Set the directory where you want to mount your project and output
OUTPUT_DIR="$(pwd)/../dist"

rm -rf $OUTPUT_DIR

# Build the Docker image
docker build -t dotnet-arm-builder -f ./raspberrypi/Dockerfile ./../..

# Run the Docker container and mount the current directory to /src and dist directory to /app/dist
docker run --rm -v $OUTPUT_DIR:/app/out dotnet-arm-builder
