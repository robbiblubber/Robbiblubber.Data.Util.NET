namespace Robbiblubber.Data.Util.OData;

using System.Text.Json.Nodes;



/// <summary>This class implements a generic OData reader.</summary>
public class ODataReader: IAsyncEnumerable<JsonObject>
{
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // protected members                                                                                                //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>Next URI.</summary>
    protected Uri? _NextUri = null;
    
    /// <summary>End of file flag.</summary>
    protected bool _Eof = false;



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // constructors                                                                                                     //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>Creates a new instance of this class.</summary>
    public ODataReader()
    {}


    /// <summary>Creates a new instance of this class.</summary>
    /// <param name="client">HTTP client.</param>
    /// <param name="uri">URI.</param>
    public ODataReader(HttpClient client, Uri uri): this(client, null, uri)
    {}


    /// <summary>Creates a new instance of this class.</summary>
    /// <param name="client">HTTP client.</param>
    /// <param name="uri">URI.</param>
    public ODataReader(HttpClient client, string uri): this(client, new Uri(uri))
    {}


    /// <summary>Creates a new instance of this class.</summary>
    /// <param name="client">HTTP client.</param>
    /// <param name="onRequest">Request callback.</param>
    /// <param name="uri">URI.</param>
    public ODataReader(HttpClient client, Action<HttpRequestMessage>? onRequest, string uri): this(client, onRequest, new Uri(uri))
    {}


    /// <summary>Creates a new instance of this class.</summary>
    /// <param name="client">HTTP client.</param>
    /// <param name="onRequest">Request callback.</param>
    /// <param name="uri">URI.</param>
    public ODataReader(HttpClient client, Action<HttpRequestMessage>? onRequest, Uri uri)
    {
        Client = client;
        OnRequest = onRequest;
        Uri = uri;
    }


    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // public properties                                                                                                //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>Resolves the next link URI.</summary>
    /// <param name="baseUri">Base URI.</param>
    /// <param name="nextLink">Next link.</param>
    /// <returns>Returns the resolved next link URI.</returns>
    private static Uri _ResolveNextLink(Uri baseUri, string nextLink)
    {
        if(Uri.TryCreate(nextLink, UriKind.Absolute, out Uri? absolute) && (absolute.Scheme is "http" or "https"))
        {
            return absolute;
        }

        Uri resolved = new(baseUri, nextLink);
        if(resolved.Scheme is not ("http" or "https")) { throw new InvalidOperationException($"Unsupported OData nextLink URI: {resolved}"); }

        return resolved;
    }



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // public properties                                                                                                //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>Gets or sets the HTTP client.</summary>
    public HttpClient? Client 
    {
        get; set; 
    }


    /// <summary>Gets or sets the URI.</summary>
    public Uri? Uri
    {
        get; set;
    }


    /// <summary>Gets or sets the request callback.</summary>
    public Action<HttpRequestMessage>? OnRequest 
    {
        get; 
        set;
    }



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // public methods                                                                                                   //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>Gets JSON content from an OData web service.</summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Returns a task that represents the asynchronous operation that returns the JSON data retrieved from the OData service..</returns>
    public virtual async Task<JsonObject> GetJsonAsync(CancellationToken ct = default)
    {
        JsonObject? firstPage = null;
        JsonArray merged = new JsonArray();

        JsonNodeOptions nodeOptions = new JsonNodeOptions() { PropertyNameCaseInsensitive = false };

        while(_NextUri != null)
        {
            using HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, _NextUri);
            OnRequest?.Invoke(req);

            using HttpResponseMessage resp = await Client!.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct).ConfigureAwait(false);
            resp.EnsureSuccessStatusCode();

            string json = await resp.Content.ReadAsStringAsync(ct).ConfigureAwait(false);

            JsonNode? node = JsonNode.Parse(json, nodeOptions);
            if(node is not JsonObject page) { throw new InvalidOperationException("Response is not a JSON object."); }

            firstPage ??= page;

            if(page["value"] is JsonArray pageValues)
            {
                foreach(JsonNode? item in pageValues) { merged.Add(item?.DeepClone()); }
            }
            else { throw new InvalidOperationException("OData response does not contain a 'value' array."); }

            string? nextLink = page["@odata.nextLink"]?.GetValue<string>();
            if(!string.IsNullOrWhiteSpace(nextLink))
            {
                _NextUri = Uri.TryCreate(nextLink, UriKind.Absolute, out Uri? abs) ? abs : new Uri(Uri!, nextLink);
            }
        }
        _Eof = true;

        JsonObject result = new JsonObject();

        foreach(KeyValuePair<string, JsonNode?> kvp in firstPage!)
        {
            if(string.Equals(kvp.Key, "value", StringComparison.Ordinal)) { continue; }
            if(string.Equals(kvp.Key, "@odata.nextLink", StringComparison.Ordinal)) { continue; }

            result[kvp.Key] = kvp.Value?.DeepClone();
        }

        result["value"] = merged;

        return result;
    }


    /// <summary>Gets the values from the OData service.</summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Returns a task that represents the asynchronous operation and returns the values retrieved from the OData service.</returns>
    public virtual async Task<JsonArray> GetValuesAsync(CancellationToken cancellationToken = default)
    {
        JsonObject page = await GetJsonAsync(cancellationToken).ConfigureAwait(false);
        if(page["value"] is not JsonArray values) { throw new InvalidOperationException("OData response does not contain a 'value' array."); }
        return values;
    }


    /// <summary>Gets the next page from the OData service.</summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Returns a task that represents the asynchronous operation and returns the next OData page, or null if there are no more pages.</returns>
    public virtual async Task<JsonObject?> NextPageAsync(CancellationToken cancellationToken = default)
    {
        if(_Eof) { return null; }
        if(_NextUri is null) { _NextUri = Uri; }
        
        using HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, _NextUri);
        OnRequest?.Invoke(req);

        using HttpResponseMessage response = await Client!.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        await using Stream stream = await response.Content.ReadAsStreamAsync(cancellationToken);

        JsonNode? rval = await JsonNode.ParseAsync(stream, cancellationToken: cancellationToken);

        string? nextLink = (rval as JsonObject)?["@odata.nextLink"]?.GetValue<string>();
        if(nextLink is null)
        {
            _NextUri = null;
            _Eof = true;
        }
        else { _NextUri = _ResolveNextLink(Uri!, nextLink); }

        return rval as JsonObject;
    }


    /// <summary>Gets the next page of values from the OData service.</summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Returns a task that represents the asynchronous operation abd returns the next page of values, or null if there are no more pages.</returns>
    public virtual async Task<JsonArray?> NextValuesAsync(CancellationToken cancellationToken = default)    
    {
        JsonObject? rval = await NextPageAsync(cancellationToken);
        return rval?["value"] as JsonArray;
    }


    /// <summary>Rewinds the reader to the initial state.</summary>
    public virtual void Rewind()
    {
        _NextUri = null;
        _Eof = false;
    }



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // [interface] IAsyncEnumerable<JsonObject>                                                                         //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    /// <summary>Gets an asynchronous enumerator that iterates asynchronously through the JSON objects retrieved from the OData service.</summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Returns an asynchronous enumerator that can be used to iterate through the retrieved JSON objects.</returns>
    async IAsyncEnumerator<JsonObject> IAsyncEnumerable<JsonObject>.GetAsyncEnumerator(CancellationToken cancellationToken)
    {
        JsonArray? page;

        while((page = await NextValuesAsync(cancellationToken)) is not null)
        {
            foreach(JsonNode? node in page)
            {
                cancellationToken.ThrowIfCancellationRequested();
                
                if(node is JsonObject obj) { yield return obj; }
            }
        }
    }
}
