using StoreFlow.Ventas.API.Datos;
using StoreFlow.Ventas.API.DTOs;

namespace StoreFlow.Ventas.API.Entidades;

public class PlanDeVentas
{
    private PlanDeVentas()
    {
    }

    public PlanDeVentas(Periodicidad periodoTiempo, decimal metaVentas, int idVendedor, string nombreVendedor)
    {
        PeriodoTiempo = periodoTiempo;
        MetaVentas = metaVentas;
        IdVendedor = idVendedor;
        NombreVendedor = nombreVendedor;
    }
    public int IdVendedor { get; private set; }
    public Periodicidad PeriodoTiempo { get; private set; }
    public decimal MetaVentas { get; private set; }
    public string NombreVendedor { get; private set; }

    public static PlanDeVentas[] CrearPlanesDeVentas(CrearPlanVentaRequest request)
    {
        var periodoTiempo = (Periodicidad)request.PeriodoTiempo;
        
        return request.Vendedores.Select(pv => 
            new PlanDeVentas(periodoTiempo, request.ValorVentas, pv.Id, pv.Nombre))
            .ToArray();
    }
}


public enum Periodicidad : byte
{
    Mensual = 1,
    Trimestral = 3,
    Semestral = 6,
    Anual = 12
}
