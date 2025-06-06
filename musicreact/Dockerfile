# 1-ci mərhələ: Build
FROM node:20-alpine AS build

WORKDIR /app
COPY ["package.json", "./"]
RUN yarn install
COPY . .
RUN yarn build

# 2-ci mərhələ: Nginx ilə serve
FROM nginx:alpine
#COPY --from=build /app/build /usr/share/nginx/html
COPY --from=build /app/dist /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
