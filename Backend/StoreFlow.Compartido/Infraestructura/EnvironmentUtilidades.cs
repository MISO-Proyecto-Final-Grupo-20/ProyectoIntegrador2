using System.Diagnostics.CodeAnalysis;

namespace StoreFlow.Compartidos.Core.Infraestructura
{
    [ExcludeFromCodeCoverage]
    public static class EnvironmentUtilidades
    {
        public static string ObtenerVariableEntornoRequerida(string nombreVariable)
        {
            var valorVariable = Environment.GetEnvironmentVariable(nombreVariable);
            if (string.IsNullOrEmpty(valorVariable))
            {
                throw new ArgumentException($"La variable de entorno '{nombreVariable}' no está configurada.");
            }

            return valorVariable;
        }
    }
}