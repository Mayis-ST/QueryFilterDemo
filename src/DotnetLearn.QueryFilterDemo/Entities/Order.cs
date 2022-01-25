namespace DotnetLearn.QueryFilterDemo.Entities;

public sealed class Order
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