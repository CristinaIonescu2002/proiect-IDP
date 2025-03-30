# build stage
FROM node:20-alpine as build
WORKDIR /app
COPY . .
RUN npm install && npm run build

# serve stage
FROM nginx:alpine
COPY --from=build /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/nginx.conf
EXPOSE 80
