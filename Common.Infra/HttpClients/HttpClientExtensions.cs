using System.Net.Http.Json;
using System.Runtime.Serialization;
using OneOf;

namespace Common.Infra.HttpClients;

public static class HttpClientExtensions
{
    /// <summary>
    /// Sends your data as json and returns response content as json with checked status code
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="method"></param>
    /// <param name="requestUri"></param>
    /// <param name="payload"></param>
    /// <param name="requestAction">Action to apply custom settings to <see cref="HttpRequestMessage"/></param>
    /// <param name="token"></param>
    /// <typeparam name="TResult">Expected type of json returned from server</typeparam>
    /// <typeparam name="TPayload">Type of request context to serialise to json</typeparam>
    /// <returns>Result or exception (see exceptions)</returns>
    /// <exception cref="HttpRequestException">When <see cref="HttpResponseMessage.IsSuccessStatusCode"/> == <c>false</c></exception>
    /// <exception cref="SerializationException">When serializer returned null</exception>
    public static async Task<OneOf<TResult, Exception>>
        SendAsJsonCheckedAsync<TResult, TPayload>(this HttpClient httpClient,
            HttpMethod method,
            Uri requestUri,
            TPayload? payload = default,
            Action<HttpRequestMessage>? requestAction = null,
            CancellationToken token = default)
    {
        var httpRequestMessage = new HttpRequestMessage(method, requestUri)
        {
            Content = payload != null ? JsonContent.Create(payload) : null
        };
        requestAction?.Invoke(httpRequestMessage);
        var response = await httpClient.SendAsync(httpRequestMessage, token);

        if (!response.IsSuccessStatusCode)
            return new HttpRequestException(response.ReasonPhrase, null, response.StatusCode);
        if (await response.Content.ReadFromJsonAsync<TResult>(cancellationToken: token) is not TResult result)
            return new SerializationException($"Request reader accidentally returned null");
        return result;
    }

    /// <inheritdoc cref="SendAsJsonCheckedAsync{TResult,TPayload}(System.Net.Http.HttpClient,System.Net.Http.HttpMethod,System.Uri,TPayload?,System.Action{System.Net.Http.HttpRequestMessage}?,System.Threading.CancellationToken)"/>
    public static Task<OneOf<TResult, Exception>>
        SendAsJsonCheckedAsync<TResult, TPayload>(this HttpClient httpClient,
            HttpMethod method,
            string requestUri,
            TPayload? payload = default,
            Action<HttpRequestMessage>? requestAction = null,
            CancellationToken token = default)
        => httpClient.SendAsJsonCheckedAsync<TResult, TPayload>(method, new Uri(requestUri, UriKind.RelativeOrAbsolute),
            payload, requestAction, token);

    /// <inheritdoc cref="SendAsJsonCheckedAsync{TResult,TPayload}(System.Net.Http.HttpClient,System.Net.Http.HttpMethod,System.Uri,TPayload?,System.Action{System.Net.Http.HttpRequestMessage}?,System.Threading.CancellationToken)"/>
    public static Task<OneOf<TResult, Exception>>
        SendAsJsonCheckedAsync<TResult>(this HttpClient httpClient,
            HttpMethod method,
            Uri requestUri,
            Action<HttpRequestMessage>? requestAction = null,
            CancellationToken token = default)
        => httpClient.SendAsJsonCheckedAsync<TResult, object>(method, requestUri, null, requestAction, token);

    /// <inheritdoc cref="SendAsJsonCheckedAsync{TResult,TPayload}(System.Net.Http.HttpClient,System.Net.Http.HttpMethod,System.Uri,TPayload?,System.Action{System.Net.Http.HttpRequestMessage}?,System.Threading.CancellationToken)"/>
    public static Task<OneOf<TResult, Exception>>
        SendAsJsonCheckedAsync<TResult>(this HttpClient httpClient,
            HttpMethod method,
            string requestUri,
            Action<HttpRequestMessage>? requestAction = null,
            CancellationToken token = default)
        => httpClient.SendAsJsonCheckedAsync<TResult, object>(method, new Uri(requestUri, UriKind.RelativeOrAbsolute),
            null, requestAction, token);
}