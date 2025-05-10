# StoreFlow - Guía de Creación y Operación de Infraestructura

Esta guía describe los pasos para desplegar y operar la infraestructura de **StoreFlow** en Azure Kubernetes Service (AKS).

---

## 🧰 Requisitos Previos

Asegúrate de tener instaladas las siguientes herramientas:

```bash
winget install --id=Hashicorp.Terraform -e
winget install --exact --id Microsoft.AzureCLI
winget install Helm.Helm
```

---

## 🚀 Despliegue de Infraestructura

### 1. Acceso y Configuración de la Suscripción

Listar suscripciones disponibles:
```powershell
az account list --output table
```

Seleccionar suscripción:
```powershell
az account set --subscription "Mi Suscripción de Producción"
```

### 2. Crear el grupo de recursos para el estado remoto de Terraform

Ejecutar script con un nombre único para la cuenta de almacenamiento:
```powershell
.\creacion-cuenta-almacenamiento-estadotf -storageAccountName "NombreUnico"
```

Actualizar el archivo `01-main.tf` con el nombre de cuenta creado:

```hcl
backend "azurerm" {
  resource_group_name   = "terraform-storage-rg"
  storage_account_name  = "nombreunicocuenta"
  container_name        = "tfstatefiles"
  key                   = "dev.terraform.tfstate"
}
```

Colocar el `subscription_id` correcto:

```hcl
provider "azurerm" {
  subscription_id = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"
  features {
    resource_group {
      prevent_deletion_if_contains_resources = false
    }
  }
}
```

### 3. Generar llaves SSH para nodos Linux

```powershell
ssh-keygen -t rsa -b 4096 -f "$env:USERPROFILE\.ssh\aks_ssh_key" -C "clave para Terraform AKS"
```

Actualizar ruta en el archivo `02-variables.tf`.

### 4. Ejecutar Terraform

```powershell
cd terraform
terraform init
terraform plan
terraform apply
```

### 5. Obtener Credenciales de AKS

```powershell
az aks get-credentials --resource-group terraform-aks-dev --name terraform-aks-dev-cluster --admin
```

---

## 🌐 Configuración del Ingress Controller

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

## 📦 Despliegue de Componentes

1. Ir a la carpeta `k8s`
2. Crear el namespace:
```powershell
kubectl apply -f namespace.yaml
```

3. Ejecutar los scripts en orden:

```powershell
01-infraestructura.ps1
02-servicios.ps1
03-ingress.ps1
```

4. Reiniciar gateway para asegurar comunicación:

```powershell
kubectl rollout restart deployment gateway -n storeflow
```

---

## 🛠 Operación del Cluster

- **Encender**:
  ```powershell
  az aks start --resource-group terraform-aks-dev --name terraform-aks-dev-cluster
  ```

- **Apagar**:
  ```powershell
  az aks stop --resource-group terraform-aks-dev --name terraform-aks-dev-cluster
  ```

- **Consultar estado**:
  ```powershell
  az aks show --resource-group terraform-aks-dev --name terraform-aks-dev-cluster --query "powerState" -o json
  ```

- **Limpiar entorno**:
  ```powershell
  kubectl delete all --all -n storeflow
  ```

- **Reiniciar servicios**:
  ```powershell
  kubectl rollout restart deployment gateway -n storeflow
  ```

- **Ver estado general**:
  ```powershell
  kubectl get all -n storeflow
  ```

- Limpiar todos los servicios del aks;
  ```PowerShell
  00-limpiar-storeflow.ps1
  ```
---

## 📱 Aplicación Móvil

```bash
nx build mobile
nx run mobile:sync:android
```

---

> ⚠️ Esta guía está orientada a desarrolladores que deseen comprender el proceso completo de despliegue y operación de StoreFlow.