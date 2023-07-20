public class CurrencyInfo
{
    public string name { get; set; }
    public string symbol { get; set; }
}

public class Name
{
    public string common { get; set; }
    public string official { get; set; }
}

public class Country
{
    public Name name { get; set; }
    public Dictionary<string, CurrencyInfo> currencies { get; set; }
    public List<string> capital { get; set; }
    public List<string> altSpellings { get; set; }
    public List<double> latlng { get; set; }
    public List<string> borders { get; set; }
    public string flag { get; set; }
    public List<string> timezones { get; set; }
    public List<string> continents { get; set; }
}

