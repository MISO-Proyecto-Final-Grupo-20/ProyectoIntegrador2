#!/bin/bash

set -e

echo "🚀 Aplicando Namespace..."
kubectl apply -f k8s/namespace.yaml

echo "🔐 Aplicando Secrets..."
kubectl apply -f k8s/secrets/shared-app-secret.yaml

echo "💾 Aplicando PersistentVolumeClaim para PostgreSQL..."
kubectl apply -f k8s/postgres/pvc.yaml

echo "🐘 Desplegando PostgreSQL..."
kubectl apply -f k8s/postgres/

echo "🧑‍💻 Desplegando servicio de usuarios..."
kubectl apply -f k8s/usuarios/

echo "✅ Despliegue completo."
