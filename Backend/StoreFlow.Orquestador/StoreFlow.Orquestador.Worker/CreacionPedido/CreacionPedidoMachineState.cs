using MassTransit;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Inventarios;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Logistica;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Ventas;

namespace StoreFlow.Orquestador.Worker.CreacionPedido;

public class CreacionPedidoMachineState : MassTransitStateMachine<CreacionPedidoState>
{
    public State ValidandoInventario { get; private set; }
    public State RegistrandoPedido { get; private set; }
    public Event<ProcesarPedido> IniciarProcesarPedido { get; private set; }
    public Event<InventarioValidado> InventarioValidado { get; private set; }
    public Event<PedidoRegistrado> PedidoRegistrado { get; set; }
    // public Event<PedidoPreparadoParaEnvio> PedidoListoParaEnvio { get; private set; }

    
    public CreacionPedidoMachineState()
    {
        InstanceState(x => x.CurrentState);

        Event(() => IniciarProcesarPedido, x =>
        {
            x.CorrelateById(context => context.Message.IdProceso);
            x.SelectId(context => context.Message.IdProceso);
        });

        Event(() => InventarioValidado, x =>
        {
            x.CorrelateById(context => context.Message.IdProceso);
            x.SelectId(context => context.Message.IdProceso);
        });


        Initially(
            When(IniciarProcesarPedido)
                .Then(contex =>
                {
                    contex.Saga.solicitud = contex.Message.solicitud;
                    contex.Publish(new ValidarInventario(contex.Message.IdProceso, contex.Message.solicitud));
                })
                .TransitionTo(ValidandoInventario)
        );

        During(ValidandoInventario,
            When(InventarioValidado)
                .Then(context =>
                {
                    context.Saga.solicitud = context.Message.SolicitudValiada;
                    context.Publish(new RegistrarPedido(context.Message.IdProceso, context.Message.SolicitudValiada));
                })
                .TransitionTo(RegistrandoPedido)
        );

        During(RegistrandoPedido,
            When(PedidoRegistrado)
                .Finalize()
        );
    }

    

    // public CreacionPedidoMachineState()
    // {
    //     InstanceState(x => x.CurrentState);
    //
    //     Event(() => IniciarProcesarPedido, x =>
    //     {
    //         x.CorrelateById(context => context.Message.IdProceso);
    //         x.SelectId(context => context.Message.IdProceso);
    //     });
    //
    //     Event(() => InventarioValidado, x =>
    //     {
    //         x.CorrelateById(context => context.Message.IdProceso);
    //         x.SelectId(context => context.Message.IdProceso);
    //     });
    //
    //     Event(() => PedidoListoParaEnvio, x =>
    //     {
    //         x.CorrelateById(context => context.Message.IdProceso);
    //         x.SelectId(context => context.Message.IdProceso);
    //     });
    //
    //
    //     Initially(
    //         When(IniciarProcesarPedido)
    //             .Then(contex => contex.Publish(new ValidarInventario(contex.Message.IdProceso)))
    //             .TransitionTo(ValidandoInventario)
    //     );
    //
    //     During(ValidandoInventario,
    //         When(InventarioValidado)
    //             .Then(context => context.Publish(new PrepararEnvioPedido(context.Message.IdProceso)))
    //             .TransitionTo(PreparandoEnvio)
    //     );
    //
    //     During(PreparandoEnvio,
    //         When(PedidoListoParaEnvio).Then(context => context.Publish(new ConfirmarPedido(context.Saga.IdPedido)))
    //             .Finalize()
    //     );
    // }
}