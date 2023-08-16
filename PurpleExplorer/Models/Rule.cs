namespace PurpleExplorer.Models;

public class Rule
{
    public Rule(string name, string filter)
    {
        Name = name;
        Filter = filter;
    }
    public string Name { get; set; }
    public string Filter { get; set; }
}