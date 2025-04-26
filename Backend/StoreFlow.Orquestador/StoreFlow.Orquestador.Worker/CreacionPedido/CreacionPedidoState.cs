using MassTransit;

namespace StoreFlow.Orquestador.Worker.CreacionPedido;

public class CreacionPedidoState : SagaStateMachineInstance, ISagaVersion
{
    public Guid CorrelationId { get; set; }
    public int Version { get; set; }
    public string CurrentState { get; set; }
    public int IdPedido { get; set; }
}