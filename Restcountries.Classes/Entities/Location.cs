public class Location
{
    public string Name { get; set; }
    public List<Location> Neighbors { get; set; }
    public List<Location> BorderCountries { get; set; }

    public Location(string name)
    {
        Name = name;
        Neighbors = new List<Location>();
        BorderCountries = new List<Location>();
    }
}
