//Console.WriteLine(Serialize(new List<JsonValue>() { new JsonValue(1), new JsonValue("kek"), new JsonValue(new Dictionary<string, JsonValue>() { { "a", new JsonValue(1) }, { "b", new JsonValue(true) }, { "c", new JsonValue(null) } }) }));
using JsonLibrary;
WeatherForecast test = new WeatherForecast();
test.TemperatureCelsius = false;
test.Date = DateTime.Now.ToString();

string json = test.Serialize();
WeatherForecast? test2 = (WeatherForecast?)json.Deserialize();
Console.WriteLine("qwe");
public class WeatherForecast
{
    public string? Date;
    public bool TemperatureCelsius;
    public string? Summary;
}

