    ### STAGE 1: Build ###
    FROM node:12.22-alpine3.10 AS build
    WORKDIR /usr/src/app
    COPY package.json package-lock.json ./
    RUN npm i -g @angular/cli@11.2.17
    
    # Install app dependencies:
    RUN npm i 
    
    COPY . .
    RUN ng build --prod
    ### STAGE 2: Run ###
    FROM nginx:1.17.1-alpine
    COPY nginx.conf nginx.conf
    COPY --from=build ./usr/src/app/dist/ /usr/share/nginx/html