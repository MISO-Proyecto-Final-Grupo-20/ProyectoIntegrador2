Write-Host "Aplicando Secrets..."
kubectl apply -f .\secrets\

Write-Host "Aplicando Bases de Datos..."
kubectl apply -f .\postgres\comprasdb\
kubectl apply -f .\postgres\inventariosdb\
kubectl apply -f .\postgres\logisticadb\
kubectl apply -f .\postgres\usuariosdb\
kubectl apply -f .\postgres\ventasdb\

Write-Host "Aplicando Infraestructura base..."
kubectl apply -f .\infraestructura\aspire\
kubectl apply -f .\infraestructura\rabbitmq\
kubectl apply -f .\infraestructura\redis\