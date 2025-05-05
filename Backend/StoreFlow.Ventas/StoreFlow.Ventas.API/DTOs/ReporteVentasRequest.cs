namespace StoreFlow.Ventas.API.DTOs;

public record ReporteVentasRequest(int? Vendedor, DateTime? FechaInicial, DateTime? FechaFinal, int? Producto);
public record ReporteVentasResponse(string Vendedor, DateTime FechaVenta, string Producto, int Cantidad);


