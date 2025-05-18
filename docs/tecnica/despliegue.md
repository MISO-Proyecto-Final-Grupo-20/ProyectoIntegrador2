# 4. Despliegue de la aplicación

Este documento describe el procedimiento para desplegar StoreFlow en Azure Kubernetes Service (AKS) utilizando Terraform, Azure CLI, Helm y kubectl. Está dirigido a desarrolladores y operadores que necesiten levantar el entorno desde cero o realizar tareas de mantenimiento.

---

## 4.1 Requisitos previos

Instalar las siguientes herramientas:

```bash
winget install --id=Hashicorp.Terraform -e
winget install --exact --id Microsoft.AzureCLI
winget install Helm.Helm
```

---

## 4.2 Provisionamiento de infraestructura con Terraform

### 1. Seleccionar la suscripción de Azure

```powershell
az account list --output table
az account set --subscription "Mi Suscripción de Producción"
```

### 2. Crear backend para estado remoto

```powershell
.\creacion-cuenta-almacenamiento-estadotf.ps1 -storageAccountName "NombreUnico"
```

Actualizar `01-main.tf` con la cuenta de almacenamiento:

```hcl
backend "azurerm" {
  resource_group_name   = "terraform-storage-rg"
  storage_account_name  = "nombreunicocuenta"
  container_name        = "tfstatefiles"
  key                   = "dev.terraform.tfstate"
}
```

### 3. Generar claves SSH

```powershell
ssh-keygen -t rsa -b 4096 -f "$env:USERPROFILE\.ssh\aks_ssh_key"
```

### 4. Ejecutar Terraform

```powershell
cd terraform
terraform init
terraform plan
terraform apply
```

### 5. Obtener credenciales del clúster

```powershell
az aks get-credentials --resource-group terraform-aks-dev --name terraform-aks-dev-cluster --admin
```

---

## 4.3 Configuración del Ingress Controller

### 1. Crear IP pública

```powershell
az group create --name aks-network-core --location "centralus"
az network public-ip create --resource-group aks-network-core --name aks-static-ip --sku Standard --allocation-method static
```

### 2. Asignar permisos al AKS

```powershell
$aksPrincipalId = az aks show --name terraform-aks-dev-cluster --resource-group terraform-aks-dev --query identity.principalId --output tsv
$subscriptionId = az account show --query id --output tsv
az role assignment create --assignee $aksPrincipalId --role "Network Contributor" --scope "/subscriptions/$subscriptionId/resourceGroups/aks-network-core"
```

### 3. Instalar Ingress con Helm

```powershell
helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
helm repo update

helm upgrade --install ingress-nginx ingress-nginx/ingress-nginx `
  --namespace ingress-nginx --create-namespace `
  --set controller.service.loadBalancerIP=<TU_IP> `
  --set controller.service.annotations."service\.beta\.kubernetes\.io/azure-load-balancer-resource-group"="aks-network-core"
```

---

## 4.4 Despliegue de microservicios

Desde la carpeta `k8s`:

1. Crear namespace:

```powershell
kubectl apply -f namespace.yaml
```

2. Ejecutar scripts de despliegue en orden:

```powershell
01-infraestructura.ps1
02-servicios.ps1
03-ingress.ps1
```

3. Reiniciar el gateway:

```powershell
kubectl rollout restart deployment gateway -n storeflow
```

---

## 4.5 Operación del clúster

Encender o apagar el clúster:

```powershell
az aks start --resource-group terraform-aks-dev --name terraform-aks-dev-cluster
az aks stop --resource-group terraform-aks-dev --name terraform-aks-dev-cluster
```

Ver estado del clúster:

```powershell
az aks show --resource-group terraform-aks-dev --name terraform-aks-dev-cluster --query "powerState" -o json
```

Reiniciar servicios:

```powershell
kubectl rollout restart deployment gateway -n storeflow
```

Eliminar recursos del namespace:

```powershell
kubectl delete all --all -n storeflow
```

Limpiar el entorno completo:

```powershell
.\00-limpiar-storeflow.ps1
```

---

## 4.6 Despliegue de la aplicación móvil

```bash
nx build mobile
nx run mobile:sync:android
```

---

> ⚠️ Nota: Este procedimiento es válido para entornos de desarrollo y pruebas. En producción se deben ajustar configuraciones sensibles como secretos, certificados TLS y límites de recursos.


[⬅️ Volver al índice](index.md)