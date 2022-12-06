using Newtonsoft.Json;

public static class ObjectExtensions
{

    public static string ToJson(this object toSerialize) => JsonConvert.SerializeObject(toSerialize);

    public static T FromJson<T>(string json) => JsonConvert.DeserializeObject<T>(json);

}
