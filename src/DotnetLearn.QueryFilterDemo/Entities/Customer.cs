namespace DotnetLearn.QueryFilterDemo.Entities;

public sealed class Customer
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