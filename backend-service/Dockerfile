FROM node:lts-stretch-slim
WORKDIR /usr/src/app
COPY . .
RUN npm install
EXPOSE 3000
CMD ["node", "main.js"]