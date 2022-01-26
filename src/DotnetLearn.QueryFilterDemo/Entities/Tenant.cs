namespace DotnetLearn.QueryFilterDemo.Entities;

using EntityMetadata;

public sealed class Tenant
    // : ISoftDeleteableEntity
{
    public Tenant(string name)
    {
        Name = name;
    }

    public int Id { get; private set; }
    public string Name { get; set; }
}