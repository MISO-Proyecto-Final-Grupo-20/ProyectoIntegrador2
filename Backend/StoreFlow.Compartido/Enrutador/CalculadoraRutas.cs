using Grpc.Core;

namespace StoreFlow.Compartidos.Core.Enrutador;

public static class CalculadoraRutas
{
    public static Dictionary<string, List<(string Direccion, Coordenada Coordenada)>> CalcularRutas(string direccionOrigen, List<string> direccionesDestino)
    {
        var coordenadaOrigen = CalcularCoordenada(direccionOrigen);
        var ubicaciones = direccionesDestino.Select(UbicarDireccion).ToList();
        var ubicacionesValidas = ubicaciones.Where(u => u.Coordenada != new Coordenada(0, 0)).ToList();

        var rutaNorte = ubicacionesValidas
            .Where(u => u.EstaAlNorteDe(coordenadaOrigen))
            .OrderBy(u => CalcularDistancia(coordenadaOrigen, u.Coordenada))
            .ToList();
        var rutaSur = ubicacionesValidas
            .Where(u => u.EstaAlNorteDe(coordenadaOrigen) == false)
            .OrderBy(u => CalcularDistancia(coordenadaOrigen, u.Coordenada))
            .ToList();
        
        return new Dictionary<string, List<(string Direccion, Coordenada Coordenada)>>
        {
            { "Norte", rutaNorte },
            { "Sur", rutaSur }
        };

    }
    
    public static bool EstaAlNorteDe(this (string Direccion, Coordenada Coordenada) ubicacion, Coordenada referencia)
    {
        return ubicacion.Coordenada.X > referencia.X;
    }

    public static (string Direccion, Coordenada Coordenada) UbicarDireccion(string direccion)
    {
        return (direccion, CalcularCoordenada(direccion));
    }
    
    public static Coordenada CalcularCoordenada(string direccion)
    {
        try
        {
            var partes = direccion.Split(' ');

            var tipoVia = partes[0].ToLower();
            var numeroCalle = int.Parse(partes[1]);
            var numeroCarrera = int.Parse(partes[3].Split('-')[0]);

            if (tipoVia is "calle" or "transversal" or "cl")
                return new Coordenada(numeroCalle, numeroCarrera);

            return new Coordenada(numeroCarrera, numeroCalle);
        }
        catch (Exception)
        {
            //Fuera del alcance 
            return new Coordenada(0, 0);
        }
    }

    public static double CalcularDistancia(Coordenada coordenada1, Coordenada coordenada2)
    {
        var deltaX = Math.Abs(coordenada2.X - coordenada1.X);
        var deltaY = Math.Abs(coordenada2.Y - coordenada1.Y);
        return Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2));
    }
}

public record Coordenada(int X, int Y);