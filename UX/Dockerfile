# Stage 1: Build React app with Node
FROM node:18-alpine AS builder

WORKDIR /app

# Copy package.json and install deps
COPY package.json package-lock.json ./
RUN npm install

# Copy source code and build
COPY . .
RUN npm run build

# Stage 2: Serve using Nginx
FROM nginx:alpine

# Copy build output from previous stage
COPY --from=builder /app/dist /usr/share/nginx/html

# Custom Nginx config (if you have it)
COPY nginx.conf /etc/nginx/conf.d/default.conf

