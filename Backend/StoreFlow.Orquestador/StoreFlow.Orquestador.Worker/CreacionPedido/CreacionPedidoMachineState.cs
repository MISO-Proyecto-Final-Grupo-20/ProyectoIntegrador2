using MassTransit;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Compras;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Inventarios;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Usuarios;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Ventas;

namespace StoreFlow.Orquestador.Worker.CreacionPedido;

public class CreacionPedidoMachineState : MassTransitStateMachine<CreacionPedidoState>
{
    public State ValidandoInventario { get; private set; }
    public State RegistrandoPedido { get; private set; }
    public State ObteniendoInformacionClienteYVendedor { get; private set; }
    public State ObteniendoInformacionProductos { get; private set; }
    public Event<ProcesarPedido> IniciarProcesarPedido { get; private set; }
    public Event<InformacionProductoObtenida> InformacionProductoObtenida { get; private set; }
    public Event<InformacionClienteYVendedorObtenida> InformacionClienteYVendedorObtenida { get; private set; }
    public Event<InventarioValidado> InventarioValidado { get; private set; }
    public Event<PedidoRegistrado> PedidoRegistrado { get; set; }

    
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

        Event(() => InformacionClienteYVendedorObtenida, x =>
        {
            x.CorrelateById(context => context.Message.IdProceso);
            x.SelectId(context => context.Message.IdProceso);
        });
        
        Event(() => InformacionProductoObtenida, x =>
        {
            x.CorrelateById(context => context.Message.IdProceso);
            x.SelectId(context => context.Message.IdProceso);
        });
        
        Event(() => PedidoRegistrado, x =>
        {
            x.CorrelateById(context => context.Message.IdProceso);
            x.SelectId(context => context.Message.IdProceso);
        });


        Initially(
            When(IniciarProcesarPedido)
                .Then(contex =>
                {
                    contex.Saga.SolicitudDePedido = contex.Message.Solicitud;
                    contex.Publish(new ValidarInventario(contex.Message.IdProceso, contex.Message.Solicitud));
                })
                .TransitionTo(ValidandoInventario)
        );

        During(ValidandoInventario,
            When(InventarioValidado)
                .Then(context =>
                {
                    context.Saga.SolicitudDePedido = context.Message.SolicitudValiada;
                    
                    context.Publish(new ObtenerInformacionClienteYVendedor(context.Message.IdProceso, context.Saga.SolicitudDePedido.IdCliente, context.Saga.SolicitudDePedido.IdVendedor));
                })
                .TransitionTo(ObteniendoInformacionClienteYVendedor)
        );
        During(ObteniendoInformacionClienteYVendedor,
            When(InformacionClienteYVendedorObtenida)
                .Then(context =>
                {
                    context.Saga.InformacionCliente = context.Message.InformacionCliente;
                    context.Saga.InformacionVendedor = context.Message.InformacionVendedor;
                    
                    var idsProductos = context.Saga.SolicitudDePedido.ProductosSolicitados.Select(p =>p.Id).ToArray();
                    context.Publish(new ObtenerInformacionProductos(context.Message.IdProceso, idsProductos));
                }).TransitionTo(ObteniendoInformacionProductos));

        During(ObteniendoInformacionProductos,
            When(InformacionProductoObtenida)
                .Then(context =>
                {
                    context.Saga.InformacionProductos = context.Message.InformacionProductos;
                    context.Publish(new RegistrarPedido(context.Message.IdProceso, context.Saga.SolicitudDePedido, context.Saga.InformacionProductos, context.Saga.InformacionCliente, context.Saga.InformacionVendedor));
                })
                .TransitionTo(RegistrandoPedido));

        During(RegistrandoPedido,
            When(PedidoRegistrado)
                .Finalize()
        );
    }

    

   
}