using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotnetLearn.QueryFilterDemo;

using Configuration;
using Entities;

public class QueryFilterContext : DbContext
{
    // private readonly int tenantId;

    public QueryFilterContext(DbContextOptions options
        // , int tenantId
        ) : base(options)
    {
        // this.tenantId = tenantId;
    }

    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<Tenant> Tenants { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var order = modelBuilder.Entity<Order>();
        var customer = modelBuilder.Entity<Customer>();
        var tenant = modelBuilder.Entity<Tenant>();

        ConfigureOrder(order);
        ConfigureCustomer(customer);

        static void ConfigureCustomer(EntityTypeBuilder<Customer> customer)
        {
            customer.HasAlternateKey(customer => (new { customer.Id, customer.TenantId }));
            customer.HasOne(customer => customer.Tenant)
                .WithMany()
                .HasForeignKey(customer => customer.TenantId);
        }

        static void ConfigureOrder(EntityTypeBuilder<Order> order)
        {
            order.HasAlternateKey(order => (new { order.Id, order.TenantId }));
            order.HasOne(order => order.Tenant)
                .WithMany()
                .HasForeignKey(order => order.TenantId);
            order.HasOne(order => order.Customer)
                .WithMany()
                .HasForeignKey(order => new { order.CustomerId, order.TenantId })
                .HasPrincipalKey(customer => new { customer.Id, customer.TenantId });
        }

        // tenant.AddSoftDeleteQueryFilter();
        // customer.AddSoftDeleteQueryFilter();
        // order.AddSoftDeleteQueryFilter();

        // modelBuilder.AddSoftDeleteQueryFilters();

        // tenant.HasQueryFilter(tenant => tenant.Id == tenantId);
        // customer.HasQueryFilter(customer => customer.TenantId == tenantId);
        // order.HasQueryFilter(order => order.TenantId == tenantId);

        // tenant.AddQueryFilter(tenant => tenant.Id == tenantId);
        // customer.AddQueryFilter(customer => customer.TenantId == tenantId);
        // order.AddQueryFilter(order => order.TenantId == tenantId);

        // modelBuilder.AddTenantQueryFiltersBad(tenantId);

        // modelBuilder.AddTenantQueryFilters(this);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        // ChangeTracker.OverrideRemovedWithSoftDeleted(DateTime.UtcNow);

        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    // used by design time tools
    public QueryFilterContext() : base() { }

    // used by design time tools, to create a demo physical version of the database
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        if (!builder.IsConfigured)
            builder.UseSqlite("Data Source=SampleDb.sqlite");
    }
}