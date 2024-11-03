namespace Bymse.Articles.PublicApi.Client;

public partial class PublicApiClient
{
    static partial void UpdateJsonSerializerSettings(Newtonsoft.Json.JsonSerializerSettings settings)
    {
        settings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
        settings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
    }

    public PublicApiClient(HttpClient client)
    {
        _httpClient = client;
        _baseUrl = "";
    }
}