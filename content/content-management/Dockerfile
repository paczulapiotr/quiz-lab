# Use the official Node.js image as the base image
FROM node:20.18-alpine

ENV NODE_ENV=production

# Set the working directory
WORKDIR /app
RUN mkdir -p public/uploads

# Install dependencies
COPY package.json package.json
COPY package-lock.json package-lock.json
RUN npm install

# Copy the backup file to the container
COPY quiz_backup.tar.gz /app/quiz_backup.tar.gz

# Copy the Strapi project files to the container
COPY config config
COPY database database
COPY src src
COPY types types
COPY .env.docker .env
COPY favicon.png favicon.png
COPY tsconfig.json tsconfig.json

RUN npm run strapi import -- -f /app/quiz_backup.tar.gz --force
RUN npm run build

# Start Strapi
CMD ["npm", "run", "start"]