#!/bin/bash
# filepath: /Users/piotrpaczula/repos/quiz/deploy/caddy/run_caddy.sh

# Get the directory where this script is located
SCRIPT_DIR=$(dirname "$(realpath "$0")")

# Change directory to the script's directory
cd "$SCRIPT_DIR" || { echo "Failed to change directory to $SCRIPT_DIR"; exit 1; }

# Run Caddy using the Caddyfile in the current directory
caddy run --config Caddyfile