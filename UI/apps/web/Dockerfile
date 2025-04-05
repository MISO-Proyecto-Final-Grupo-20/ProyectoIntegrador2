FROM node:20-slim AS builder

WORKDIR /app

RUN apt-get update && apt-get install -y python3 make g++ && rm -rf /var/lib/apt/lists/*

COPY package.json ./
COPY nx.json ./
COPY tsconfig.base.json ./
COPY apps/web ./apps/web
COPY libs ./libs

RUN npm install --include=optional

RUN npx nx build web --configuration=production

FROM nginx:alpine

COPY --from=builder /app/dist/apps/web/browser /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]