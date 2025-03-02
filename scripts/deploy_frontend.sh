#!/bin/bash

# Get the directory of this script
SCRIPT_DIR=$(dirname "$(realpath "$0")")

# Change directory to ../frontend and run build
cd "$SCRIPT_DIR/../frontend" || { echo "Failed to change directory to ../frontend"; exit 1; }
echo "Running npm run build in $(pwd)..."
npm run build

# Define source and destination directories for slave and master apps
SLAVE_SRC="$SCRIPT_DIR/../frontend/apps/slave/dist"
SLAVE_DEST="$SCRIPT_DIR/../deploy/caddy/slave_ui/dist"
MASTER_SRC="$SCRIPT_DIR/../frontend/apps/master/dist"
MASTER_DEST="$SCRIPT_DIR/../deploy/caddy/master_ui/dist"

# Ensure destination directories exist
mkdir -p "$SLAVE_DEST" "$MASTER_DEST"

# Clean destination directories before copying
echo "Cleaning $SLAVE_DEST and $MASTER_DEST..."
rm -rf "$SLAVE_DEST"/* "$MASTER_DEST"/*

# Copy built files for slave app
echo "Copying slave app from $SLAVE_SRC to $SLAVE_DEST..."
cp -r "$SLAVE_SRC"/* "$SLAVE_DEST"

# Copy built files for master app
echo "Copying master app from $MASTER_SRC to $MASTER_DEST..."
cp -r "$MASTER_SRC"/* "$MASTER_DEST"

echo "Deployment complete."