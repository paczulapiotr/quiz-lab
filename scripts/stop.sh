#!/bin/bash

# Get the directory of the script
SCRIPT_DIR=$(dirname "$(realpath "$0")")

docker compose -f "$SCRIPT_DIR/../e2e.docker-compose.yaml" down
