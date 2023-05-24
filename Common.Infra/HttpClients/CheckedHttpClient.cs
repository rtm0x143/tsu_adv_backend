using System.Net.Http.Json;
using System.Runtime.Serialization;
using Common.Domain.ValueTypes;
using OneOf;

namespace Common.Infra.HttpClients;

public class CheckedHttpClient
{
    protected readonly HttpClient HttpClient;
    public CheckedHttpClient(HttpClient httpClient) => HttpClient = httpClient;

    /// <summary>
    /// Sends your data as json and returns response content as json with checked status code
    /// </summary>
    /// <param name="method">Http method</param>
    /// <param name="requestUri">Request url</param>
    /// <param name="payload">Request body data</param>
    /// <param name="requestAction">Action to apply custom settings to <see cref="HttpRequestMessage"/></param>
    /// <param name="token">cancellation token of asynchronous operation</param>
    /// <typeparam name="TResult">Expected type of json returned from server</typeparam>
    /// <typeparam name="TPayload">Type of request context to serialise to json</typeparam>
    /// <returns>Result or exception (see exceptions)</returns>
    /// <exception cref="HttpRequestException">When <see cref="HttpResponseMessage.IsSuccessStatusCode"/> == <c>false</c></exception>
    /// <exception cref="SerializationException">When serializer returned null</exception>
    protected virtual async Task<OneOf<TResult, Exception>>
        SendAsJsonCheckedAsync<TResult, TPayload>(
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
        var response = await HttpClient.SendAsync(httpRequestMessage, token);

        if (!response.IsSuccessStatusCode)
            return new HttpRequestException(response.ReasonPhrase, null, response.StatusCode);

        if (typeof(TResult) == typeof(EmptyResult)) return default;

        if (await response.Content.ReadFromJsonAsync<TResult>(cancellationToken: token) is not TResult result)
            return new SerializationException($"Request reader accidentally returned null");
        return result;
    }

    /// <inheritdoc cref="SendAsJsonCheckedAsync{TResult,TPayload}(System.Net.Http.HttpMethod,System.Uri,TPayload?,System.Action{System.Net.Http.HttpRequestMessage}?,System.Threading.CancellationToken)"/>
    public Task<OneOf<TResult, Exception>> SendAsJsonCheckedAsync<TResult, TPayload>(
        HttpMethod method,
        string requestUri,
        TPayload? payload = default,
        Action<HttpRequestMessage>? requestAction = null,
        CancellationToken token = default)
        => SendAsJsonCheckedAsync<TResult, TPayload>(
            method, new Uri(requestUri, UriKind.RelativeOrAbsolute), payload, requestAction, token);

    /// <inheritdoc cref="SendAsJsonCheckedAsync{TResult,TPayload}(System.Net.Http.HttpMethod,System.Uri,TPayload?,System.Action{System.Net.Http.HttpRequestMessage}?,System.Threading.CancellationToken)"/>
    public Task<OneOf<TResult, Exception>> SendAsJsonCheckedAsync<TResult>(
        HttpMethod method,
        Uri requestUri,
        Action<HttpRequestMessage>? requestAction = null,
        CancellationToken token = default)
        => SendAsJsonCheckedAsync<TResult, object>(method, requestUri, null, requestAction, token);

    /// <inheritdoc cref="SendAsJsonCheckedAsync{TResult,TPayload}(System.Net.Http.HttpMethod,System.Uri,TPayload?,System.Action{System.Net.Http.HttpRequestMessage}?,System.Threading.CancellationToken)"/>
    public Task<OneOf<TResult, Exception>> SendAsJsonCheckedAsync<TResult>(HttpMethod method,
        string requestUri,
        Action<HttpRequestMessage>? requestAction = null,
        CancellationToken token = default)
        => SendAsJsonCheckedAsync<TResult, object>(
            method, new Uri(requestUri, UriKind.RelativeOrAbsolute), null, requestAction, token);

    /// <inheritdoc cref="SendAsJsonCheckedAsync{TResult,TPayload}(System.Net.Http.HttpMethod,System.Uri,TPayload?,System.Action{System.Net.Http.HttpRequestMessage}?,System.Threading.CancellationToken)"/>
    public Task<OneOf<EmptyResult, Exception>> SendAsJsonCheckedAsync<TPayload>(
        HttpMethod method,
        Uri requestUri,
        TPayload payload,
        Action<HttpRequestMessage>? requestAction = null,
        CancellationToken token = default)
        => SendAsJsonCheckedAsync<EmptyResult, TPayload>(method, requestUri, payload, requestAction, token);

    /// <inheritdoc cref="SendAsJsonCheckedAsync{TResult,TPayload}(System.Net.Http.HttpMethod,System.Uri,TPayload?,System.Action{System.Net.Http.HttpRequestMessage}?,System.Threading.CancellationToken)"/>
    public Task<OneOf<EmptyResult, Exception>> SendAsJsonCheckedAsync<TPayload>(
        HttpMethod method,
        string requestUri,
        TPayload payload,
        Action<HttpRequestMessage>? requestAction = null,
        CancellationToken token = default)
        => SendAsJsonCheckedAsync<EmptyResult, TPayload>(
            method, new Uri(requestUri, UriKind.RelativeOrAbsolute), payload, requestAction, token);

    /// <inheritdoc cref="SendAsJsonCheckedAsync{TResult,TPayload}(System.Net.Http.HttpMethod,System.Uri,TPayload?,System.Action{System.Net.Http.HttpRequestMessage}?,System.Threading.CancellationToken)"/>
    public Task<OneOf<EmptyResult, Exception>> SendCheckedAsync(
        HttpMethod method,
        Uri requestUri,
        Action<HttpRequestMessage>? requestAction = null,
        CancellationToken token = default)
        => SendAsJsonCheckedAsync<EmptyResult, object>(method, requestUri, null, requestAction, token);

    /// <inheritdoc cref="SendAsJsonCheckedAsync{TResult,TPayload}(System.Net.Http.HttpMethod,System.Uri,TPayload?,System.Action{System.Net.Http.HttpRequestMessage}?,System.Threading.CancellationToken)"/>
    public Task<OneOf<EmptyResult, Exception>> SendCheckedAsync(
        HttpMethod method,
        string requestUri,
        Action<HttpRequestMessage>? requestAction = null,
        CancellationToken token = default)
        => SendAsJsonCheckedAsync<EmptyResult, object>(
            method, new Uri(requestUri, UriKind.RelativeOrAbsolute), null, requestAction, token);
}