using MassTransit.Testing;
using NSubstitute;
using StoreFlow.Compartidos.Core.Mensajes.CreacionPedido.Usuarios;
using StoreFlow.Usuarios.API.Consumidores;
using StoreFlow.Usuarios.API.Servicios;

public class ObtenerInformacionClienteYVendedorConsumerTests
{
    [Fact]
    public async Task Consume_DeberiaPublicarInformacionClienteYVendedorObtenida()
    {
        // Arrange
        var usuariosServicios = Substitute.For<IUsuariosServicios>();
        var idCliente = 1;
        var idVendedor = 5;
        var idProceso = Guid.NewGuid();

        var informacionCliente = new InformacionCliente(idCliente, "Direccion Cliente", "Nombre Cliente");
        var informacionVendedor = new InformacionVendedor(idVendedor, "Nombre Vendedor");

        
        usuariosServicios.ObtenerInformacionClienteYVendedor(idCliente, idVendedor)
            .Returns((informacionCliente, informacionVendedor));
        
        var harness = new InMemoryTestHarness();
        var consumerHarness = harness.Consumer(() => new ObtenerInformacionClienteYVendedorConsumer(usuariosServicios));

        await harness.Start();
        try
        {
            // Act
            await harness.InputQueueSendEndpoint.Send(new ObtenerInformacionClienteYVendedor(idProceso, idCliente, idVendedor));

            // Assert
            Assert.True(await harness.Consumed.Any<ObtenerInformacionClienteYVendedor>());
            Assert.True(await consumerHarness.Consumed.Any<ObtenerInformacionClienteYVendedor>());

            var publishedMessage = harness.Published.Select<InformacionClienteYVendedorObtenida>().FirstOrDefault();
            Assert.NotNull(publishedMessage);
            Assert.Equal(idProceso, publishedMessage.Context.Message.IdProceso);
            Assert.Equal(informacionCliente, publishedMessage.Context.Message.InformacionCliente);
            Assert.Equal(informacionVendedor, publishedMessage.Context.Message.InformacionVendedor);
        }
        finally
        {
            await harness.Stop();
        }
    }
}