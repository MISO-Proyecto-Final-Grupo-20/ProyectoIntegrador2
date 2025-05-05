namespace StoreFlow.Ventas.API.DTOs;

public record CrearPlanVentaRequest(byte PeriodoTiempo, decimal ValorVentas, VendedorPlanVentas[] Vendedores);
public record VendedorPlanVentas(int Id, string Nombre);

public record PeriodoTiempoResponse(int Id, string Descripcion);