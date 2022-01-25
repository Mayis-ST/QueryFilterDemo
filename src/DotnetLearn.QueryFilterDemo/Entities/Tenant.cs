namespace DotnetLearn.QueryFilterDemo.Entities;

public sealed class Tenant
{
    public Tenant(string name)
    {
        Name = name;
    }

    public int Id { get; private set; }
    public string Name { get; set; }
}