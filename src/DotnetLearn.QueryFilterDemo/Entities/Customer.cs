namespace DotnetLearn.QueryFilterDemo.Entities;

using EntityMetadata;

public sealed class Customer
    // : ISoftDeleteableEntity
    // , ITenantSpecific
{
    public Customer(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public int TenantId { get; set; }

    public Tenant Tenant { get; set; } = null!;
}