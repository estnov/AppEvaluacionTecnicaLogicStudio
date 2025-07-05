FROM node:20-alpine AS builder

WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .
RUN npm run build -- --configuration production
FROM nginx:1.27-alpine

COPY --from=builder /app/dist/front-test-logic-studio/browser/ /usr/share/nginx/html

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]