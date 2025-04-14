FROM node:20-slim AS builder

WORKDIR /app

RUN apt-get update && apt-get install -y python3 make g++ && rm -rf /var/lib/apt/lists/*

COPY package.json ./
COPY nx.json ./
COPY tsconfig.base.json ./
COPY apps/mobile ./apps/mobile
COPY libs ./libs

RUN npm install --include=optional --legacy-peer-deps --force

ARG ENVIRONMENT=production
RUN npx nx build mobile --configuration=$ENVIRONMENT

FROM nginx:alpine

COPY --from=builder /app/dist/apps/mobile/browser /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
