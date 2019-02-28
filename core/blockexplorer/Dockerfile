FROM node:10.3.0 as build-dependencies

# Note this is for a standalone instance of the blockexplorer in its own docker container

WORKDIR /usr/src/app
COPY package.json yarn.lock ./
RUN yarn
COPY . ./
RUN yarn build

FROM nginx:1.14
RUN rm -rf /etc/nginx/conf.d
COPY conf /etc/nginx
COPY --from=build-dependencies /usr/src/app/build /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]

