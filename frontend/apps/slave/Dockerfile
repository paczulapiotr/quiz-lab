# Use an official Node.js runtime as the base image
FROM node:22-alpine3.20 AS base

FROM base AS builder

RUN apk update
RUN apk add --no-cache libc6-compat
# Set working directory
WORKDIR /app
RUN npm i --global turbo
COPY . .
RUN turbo prune slave --docker

# Add lockfile and package.json's of isolated subworkspace
FROM base AS installer
RUN apk update
RUN apk add --no-cache libc6-compat
WORKDIR /app

# First install dependencies (as they change less often)
COPY --from=builder /app/out/json/ .
RUN npm install

# Build the project and its dependencies
ARG VITE_LOCAL_API_URL
ENV VITE_LOCAL_API_URL=$VITE_LOCAL_API_URL

COPY --from=builder /app/out/full/ .
RUN npx turbo build

# Use an official Nginx image to serve the build files
FROM nginx:1.27-alpine

# Copy the build output to the Nginx HTML directory
COPY --from=installer /app/apps/slave/dist/ /usr/share/nginx/html

# Copy the custom Nginx configuration file
COPY ./apps/master/nginx.conf /etc/nginx/conf.d/default.conf

# Expose port 80 to serve the app
EXPOSE 80

# Start Nginx when the container runs
CMD ["nginx", "-g", "daemon off;"]