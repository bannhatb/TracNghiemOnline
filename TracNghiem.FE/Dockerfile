FROM node:14-alpine AS base
WORKDIR /src
COPY . .
RUN npm install
RUN npm install -g @angular/cli

FROM base AS development
WORKDIR /src
RUN ng build --prod
EXPOSE 4200
# EXPOSE 49153
CMD ["ng", "serve", "--host", "0.0.0.0", "--poll", "500"]

FROM nginx:alpine AS production
WORKDIR /src
RUN rm /etc/nginx/conf.d/default.conf
COPY nginx.conf /etc/nginx/conf.d/
COPY --from=development /src/dist/UI /usr/share/nginx/html
EXPOSE 80
