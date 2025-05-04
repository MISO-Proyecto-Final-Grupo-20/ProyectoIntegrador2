namespace StoreFlow.Ventas.API.DTOs;

public record CrearPlanVentaRequest(byte periodoTiempo, decimal valorVentas, VendedorPlanVentas[] vendedores);
public record VendedorPlanVentas(int idVendedor, string nombreVendedor);

public record PeriodoTiempoResponse(int Id, string Descripcion);