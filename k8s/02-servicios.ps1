Write-Host "Aplicando Servicios Backend..."
kubectl apply -f .\compras\
kubectl apply -f .\inventarios\
kubectl apply -f .\logistica\
kubectl apply -f .\usuarios\
kubectl apply -f .\ventas\
kubectl apply -f .\orquestador\

Write-Host "Aplicando Gateway y Web..."
kubectl apply -f .\gateway\
kubectl apply -f .\web\