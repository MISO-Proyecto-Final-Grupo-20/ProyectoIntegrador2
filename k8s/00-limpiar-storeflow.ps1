Write-Host "Eliminando todos los recursos principales (Pods, Deployments, Services, etc.)..."
kubectl delete all --all -n storeflow

Write-Host "Eliminando todos los Ingress..."
kubectl delete ingress --all -n storeflow

Write-Host "Eliminando todos los PersistentVolumeClaims (PVCs)..."
kubectl delete pvc --all -n storeflow

Write-Host "Eliminando todos los Secrets..."
kubectl delete secret --all -n storeflow

Write-Host "Limpieza completa del namespace 'storeflow' finalizada."