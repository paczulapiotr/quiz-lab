#!/bin/bash

# Default values
gameSize=1
gameIdentifier="test_game"
locale=1

# Parse command-line arguments
while getopts "s:i:l:" opt; do
  case $opt in
    s) gameSize=$OPTARG ;;
    i) gameIdentifier=$OPTARG ;;
    l) locale=$OPTARG ;;
    \?) echo "Invalid option -$OPTARG" >&2 ;;
  esac
done

# URL to send the POST request to
URL="http://localhost:5999/api/game/create"

# JSON body to send in the POST request
JSON_BODY=$(cat <<EOF
{
    "gameSize": $gameSize,
    "gameIdentifier": "$gameIdentifier",
    "locale": $locale
}
EOF
)

# Send the POST request using curl
curl -X POST "$URL" \
-H "Content-Type: application/json" \
-d "$JSON_BODY"