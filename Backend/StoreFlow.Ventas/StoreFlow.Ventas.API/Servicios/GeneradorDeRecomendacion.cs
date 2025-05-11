using System.Diagnostics.CodeAnalysis;
using OpenAI.Chat;

namespace StoreFlow.Ventas.API.Servicios;

[ExcludeFromCodeCoverage]
public class GeneradorDeRecomendacion : IGeneradorDeRecomendacion
{
    private readonly ChatClient _chatClient;

    public GeneradorDeRecomendacion(ChatClient chatClient)
    {
        _chatClient = chatClient;
    }

    public async Task<string> GenerarAsync(string prompt)
    {
        var mensajes = new List<ChatMessage>
        {
            new SystemChatMessage(
                "Eres un experto en trade marketing. Analizas tiendas de barrio, supermercados o distribuidores y das recomendaciones claras, útiles y concisas (máximo 300 caracteres)."),
            new UserChatMessage(
                $"{prompt} Por favor, responde en texto plano, sin formato markdown, en una sola línea, separando cada recomendación con comas. No uses listas, guiones ni saltos de línea. Máximo 300 caracteres.")
        };

        var requestOptions = new ChatCompletionOptions
        {
            MaxOutputTokenCount = 500,
            Temperature = 0.8f,
            TopP = 1.0f
        };

        var response = await _chatClient.CompleteChatAsync(mensajes, requestOptions);

        return response.Value.Content[0].Text;
    }
}