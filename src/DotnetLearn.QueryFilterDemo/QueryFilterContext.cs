namespace DotnetLearn.QueryFilterDemo;

using Entities;

public class QueryFilterContext : DbContext
{
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<Tenant> Tenants { get; set; } = null!;

    public QueryFilterContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var order = modelBuilder.Entity<Order>();
        order.HasAlternateKey(order => (new { order.Id, order.TenantId }));
        order.HasOne(order => order.Tenant)
            .WithMany()
            .HasForeignKey(order => order.TenantId);
        order.HasOne(order => order.Customer)
            .WithMany()
            .HasForeignKey(order => new { order.CustomerId, order.TenantId })
            .HasPrincipalKey(customer => new { customer.Id, customer.TenantId });

        var customer = modelBuilder.Entity<Customer>();
        customer.HasAlternateKey(customer => (new { customer.Id, customer.TenantId }));
        customer.HasOne(customer => customer.Tenant)
            .WithMany()
            .HasForeignKey(customer => customer.TenantId);
    }
}
