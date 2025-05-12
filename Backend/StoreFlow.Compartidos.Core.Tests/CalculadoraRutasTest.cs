using StoreFlow.Compartidos.Core.Enrutador;

namespace StoreFlow.Compartidos.Core.Tests;

public class CalculadoraRutasTest
{
    [Fact]
    public void CalculaCoordenadasPor_Calle()
    {
        var direccion = "Calle 17 # 45-67";
        var coordenada = CalculadoraRutas.CalcularCoordenada(direccion);
        Assert.Equal(new Coordenada(17, 45), coordenada);
    }
    
    [Fact]
    public void UbicarDireccion()
    {
        var direccion = "Calle 17 # 45-67";
        var coordenada = CalculadoraRutas.UbicarDireccion(direccion);
        Assert.Equal((direccion, new Coordenada(17, 45)), coordenada);
    }
    
    [Fact]
    public void CalculaCoordenadasPor_CL()
    {
        var direccion = "CL 17 # 45-67";
        var coordenada = CalculadoraRutas.CalcularCoordenada(direccion);
        Assert.Equal(new Coordenada(17, 45), coordenada);
    }
    
    [Fact]
    public void CalculaCoordenadasPor_calle()
    {
        var direccion = "calle 17 # 45-67";
        var coordenada = CalculadoraRutas.CalcularCoordenada(direccion);
        Assert.Equal(new Coordenada(17, 45), coordenada);
    }
    
    [Fact]
    public void CalculaCoordenadasPor_transversal()
    {
        var direccion = "transversal 17 # 45-67";
        var coordenada = CalculadoraRutas.CalcularCoordenada(direccion);
        Assert.Equal(new Coordenada(17, 45), coordenada);
    }
    
    [Fact]
    public void CalculaCoordenadasPor_Carrera()
    {
        var direccion = "Carrera 58 # 11-15";
        var coordenada = CalculadoraRutas.CalcularCoordenada(direccion);
        Assert.Equal(new Coordenada(11, 58), coordenada);
    }
    
    [Fact]
    public void CalculaCoordenadasPor_carrera()
    {
        var direccion = "carrera 58 # 11-15";
        var coordenada = CalculadoraRutas.CalcularCoordenada(direccion);
        Assert.Equal(new Coordenada(11, 58), coordenada);
    }
    
    [Fact]
    public void CalculaCoordenadasPor_Diagonal()
    {
        var direccion = "Diagonal 58 # 11-15";
        var coordenada = CalculadoraRutas.CalcularCoordenada(direccion);
        Assert.Equal(new Coordenada(11, 58), coordenada);
    }
    
    [Fact]
    public void CalcularDistanciaEntreCoordenadas()
    {
        var coordenada1 = new Coordenada(10, 20);
        var coordenada2 = new Coordenada(30, 40);
        var distancia = CalculadoraRutas.CalcularDistancia(coordenada1, coordenada2);
        Assert.Equal(28.284271247461902, distancia, 6);
    }
    [Fact]
    public void CalcularDistanciaEntreCoordenadasConValoresNegativos()
    {
        var coordenada1 = new Coordenada(-10, -20);
        var coordenada2 = new Coordenada(-30, -40);
        var distancia = CalculadoraRutas.CalcularDistancia(coordenada1, coordenada2);
        Assert.Equal(28.284271247461902, distancia, 6);
    }
    [Fact]
    public void CalcularDistanciaEntreCoordenadasConValoresMixtos()
    {
        var coordenada1 = new Coordenada(-10, 20);
        var coordenada2 = new Coordenada(30, -40);
        var distancia = CalculadoraRutas.CalcularDistancia(coordenada1, coordenada2);
        Assert.Equal(72.111025999999995, distancia, 6);
    }

    [Fact]
    public void CalcularRutas()
    {
        var direccionOrigen = "Calle 100 # 45-67";
        var direccionesDestino = new List<string>
        {
            "Calle 17 # 45-67",
            "Carrera 58 # 127-15",
            "CL 153 # 11-15",
            "Al lado del mueso nacional",
        };
        var rutas = CalculadoraRutas.CalcularRutas(direccionOrigen, direccionesDestino);
        
        Assert.Equal(2, rutas["Norte"].Capacity);
        Assert.Equal(1, rutas["Sur"].Capacity);
        Assert.Equal("Carrera 58 # 127-15", rutas["Norte"][0].Direccion);
        Assert.Equal("CL 153 # 11-15", rutas["Norte"][1].Direccion);
    }
}