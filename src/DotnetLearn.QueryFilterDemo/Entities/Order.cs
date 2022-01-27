namespace DotnetLearn.QueryFilterDemo.Entities;

using EntityMetadata;

public sealed class Order
    // : ISoftDeleteableEntity
    // , ITenantSpecific
{
    public Order(decimal amount)
    {
        Amount = amount;
    }

    public int Id { get; private set; }
    public decimal Amount { get; set; }

    public int CustomerId { get; set; }
    public int TenantId { get; set; }

    public Tenant Tenant { get; set; } = null!;
    public Customer Customer { get; set; } = null!;
}