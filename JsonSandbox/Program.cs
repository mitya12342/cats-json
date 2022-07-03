using JsonLibrary;

//Вроде работает с простыми типами

WeatherForecast test = new();
test.SunIsADeadlyLaser = false;
test.Temp = 25;
test.Test = "qweewq";

string json = test.Serialize();
Console.WriteLine(json);

WeatherForecast? test2 = (WeatherForecast?)json.Deserialize();

//Не доделано (Вложенные generic коллекции)

Week Week = new();
Week.WeekForecast.Add(test);
Console.WriteLine(Week.Serialize());

Dictionary<string, List<int>> test_root = new() { { "a", new List<int>() { 1, 2, 3 } }, { "b", new List<int>() { 3, 2, 1 } } };

Console.WriteLine(test_root.Serialize()); 



public class WeatherForecast
{
    public int Temp;
    public bool SunIsADeadlyLaser;
    public string? Summary;
    public string? Test;
}

public class Week
{
    public List<WeatherForecast> WeekForecast = new();
}
