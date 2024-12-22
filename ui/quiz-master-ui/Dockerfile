# Use an official Node.js runtime as the base image
FROM node:22-alpine3.20

ARG VITE_LOCAL_API_URL
ENV VITE_LOCAL_API_URL=$VITE_LOCAL_API_URL

# Set the working directory inside the container
WORKDIR /app/quiz-master-ui

# Copy package.json and package-lock.json to install dependencies
COPY ./quiz-master-ui/package*.json ./

# Install dependencies
RUN npm install

# Copy the rest of the application code to the working directory
COPY ./quiz-master-ui/. .
COPY ./quiz-common-ui/package.json ../quiz-common-ui/package.json
COPY ./quiz-common-ui/dist/ ../quiz-common-ui/dist/

# Build the Vite React app
RUN npm run build

# Use an official Nginx image to serve the build files
FROM nginx:1.27-alpine

# Copy the build output to the Nginx HTML directory
COPY --from=0 /app/dist /usr/share/nginx/html

# Copy the custom Nginx configuration file
COPY ./quiz-master-ui/nginx.conf /etc/nginx/conf.d/default.conf

# Expose port 80 to serve the app
EXPOSE 80

# Start Nginx when the container runs
CMD ["nginx", "-g", "daemon off;"]