//Console.WriteLine(Serialize(new List<JsonValue>() { new JsonValue(1), new JsonValue("kek"), new JsonValue(new Dictionary<string, JsonValue>() { { "a", new JsonValue(1) }, { "b", new JsonValue(true) }, { "c", new JsonValue(null) } }) }));
using JsonLibrary;
WeatherForecast test = new WeatherForecast();
test.TemperatureCelsius = 25;
Console.WriteLine(test.Serialize());

public class WeatherForecast
{
    public string? Date;
    public int TemperatureCelsius;
    public string? Summary;
}

