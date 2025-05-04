using MassTransit;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Compras;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Usuarios;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Ventas;

namespace StoreFlow.Orquestador.Worker.CreacionPedido;

public class CreacionPedidoState : SagaStateMachineInstance, ISagaVersion
{
    public Guid CorrelationId { get; set; }
    public int Version { get; set; }
    public string CurrentState { get; set; }
    public SolicitudDePedido SolicitudDePedido { get; set; }
    public List<InformacionPoducto> InformacionProductos { get; set; }
    public InformacionCliente InformacionCliente { get; set; }
    public InformacionVendedor? InformacionVendedor { get; set; }
    
}