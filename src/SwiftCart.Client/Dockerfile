FROM node:20-alpine AS build
WORKDIR /src/Web-Spa
COPY . .
RUN npm install
EXPOSE 4200
CMD ["npm", "start"]

