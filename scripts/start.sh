#!/bin/bash

# Get the directory of the script
SCRIPT_DIR=$(dirname "$(realpath "$0")")

# Ensure Docker Compose services are not running
docker compose -f "$SCRIPT_DIR/../e2e.docker-compose.yaml" down

# Build and start the services defined in e2e.docker-compose.yaml
docker compose -f "$SCRIPT_DIR/../e2e.docker-compose.yaml" up --build -d

# Function to check if a service is healthy
check_service_health() {
  local service=$1
  local status=$(docker inspect --format='{{.State.Health.Status}}' $(docker compose -f "$SCRIPT_DIR/../e2e.docker-compose.yaml" ps -q $service))
  if [ "$status" == "healthy" ]; then
    return 0
  else
    return 1
  fi
}

# Wait for the services to be healthy
services=("quiz-rabbitmq")
for service in "${services[@]}"; do
  echo "Waiting for $service to be healthy..."
  while ! check_service_health $service; do
    sleep 3
  done
  echo "$service is healthy."
done

# Open Chrome instances with specified URLs
open -na "Google Chrome" --args --new-window "http://localhost:6010"
open -na "Google Chrome" --args --new-window "http://localhost:6011"
open -na "Google Chrome" --args --new-window "http://localhost:6012"
open -na "Google Chrome" --args --new-window "http://localhost:6013"
open -na "Google Chrome" --args --new-window "http://localhost:6030"