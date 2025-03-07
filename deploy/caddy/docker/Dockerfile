# Use the official Caddy image as the base image
FROM caddy:2.9.1-alpine

# Set the working directory
WORKDIR /app

# Copy the Caddyfile to the container
COPY docker/Caddyfile /etc/caddy/Caddyfile

# Copy the master_ui and slave_ui folders to the container
COPY master_ui /app/master_ui
COPY slave_ui /app/slave_ui

# Expose port 4000
EXPOSE 4000

# Start Caddy
CMD ["caddy", "run", "--config", "/etc/caddy/Caddyfile", "--adapter", "caddyfile"]