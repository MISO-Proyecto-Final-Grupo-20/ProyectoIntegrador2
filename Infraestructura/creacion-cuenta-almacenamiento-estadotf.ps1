param (
    [Parameter(Mandatory = $true)]
    [string]$storageAccountName
)

# Variables fijas
$resourceGroupName = "terraform-storage-rg"
$location = "eastus"
$containerName = "tfstatefiles"

# Mostrar la suscripción activa
$currentSub = az account show --output json | ConvertFrom-Json
Write-Host "🔄 Suscripción activa: $($currentSub.name) ($($currentSub.id))" -ForegroundColor Cyan

# Verificar disponibilidad del nombre de la cuenta
$nameCheck = az storage account check-name --name $storageAccountName | ConvertFrom-Json
if (-not $nameCheck.nameAvailable) {
    Write-Host "❌ El nombre '$storageAccountName' no está disponible. Intenta con otro." -ForegroundColor Red
    return
}

# Crear grupo de recursos
Write-Host "📁 Creando (o reutilizando) el grupo de recursos '$resourceGroupName'..."
az group create --name $resourceGroupName --location $location | Out-Null

# Crear cuenta de almacenamiento
Write-Host "💾 Creando la cuenta de almacenamiento '$storageAccountName'..."
az storage account create `
  --name $storageAccountName `
  --resource-group $resourceGroupName `
  --location $location `
  --sku Standard_LRS `
  --kind StorageV2 `
  --encryption-services blob | Out-Null

# Obtener la clave de acceso para crear el contenedor
$storageKey = az storage account keys list `
  --account-name $storageAccountName `
  --resource-group $resourceGroupName `
  --query "[0].value" `
  --output tsv

# Crear contenedor tfstatefiles con validación
Write-Host "📦 Creando el contenedor '$containerName'..."
$containerResult = az storage container create `
  --name $containerName `
  --account-name $storageAccountName `
  --account-key $storageKey `
  --output json | ConvertFrom-Json

if ($null -eq $containerResult -or -not $containerResult.created) {
    Write-Host "⚠️ El contenedor '$containerName' ya existe o no pudo ser creado." -ForegroundColor Yellow
} else {
    Write-Host "✅ Contenedor '$containerName' creado correctamente." -ForegroundColor Green
}
