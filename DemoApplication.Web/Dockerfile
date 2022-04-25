# 1. Create a Docker image with the built angular assets
FROM nginx:1.15.8-alpine
COPY nginx.conf /etc/nginx/nginx.conf
COPY /dist/DemoApplication /usr/share/nginx/html
