using System.Text.Json;
using System.Text.Json.Serialization;

/*
//Gotten from here: https://passos.com.au/converting-json-object-into-c-list/
//Actuall microsoft doc: https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-converters-how-to?pivots=dotnet-6-0
public class AdminConverter : JsonConverter
{
    // This is used when you're converting the C# List back to a JSON format
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        foreach (var player in (List<Player>)value)
        {
            writer.WriteRawValue(JsonConvert.SerializeObject(player));
        }
        writer.WriteEndArray();
    }

    // This is when you're reading the JSON object and converting it to C#
    public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
        JsonSerializer serializer)
    {
        var response = new List<Player>();
        // Loading the JSON object
        JObject players = JObject.Load(reader);
        // Looping through all the properties. C# treats it as key value pair
        foreach (var player in players)
        {
            // Finally I'm deserializing the value into an actual Player object
            var p = JsonConvert.DeserializeObject<Player>(player.Value.ToString());
            // Also using the key as the player Id
            p.Id = player.Key;
            response.Add(p);
        }

        return response;
    }

    public override bool CanConvert(Type objectType) => objectType == typeof(List<Player>);
}
*/