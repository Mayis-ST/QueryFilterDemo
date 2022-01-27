namespace DotnetLearn.QueryFilterDemo.Tests;

using Utility;

public class QueryFilterContextTests : IDisposable
{
    private const int TenantId = 1;
    private readonly SqliteInMemoryOptionsProvider inMemory = new ();
    private readonly QueryFilterContext systemUnderTest;

    public QueryFilterContextTests()
    {
        systemUnderTest = CreateContext();
        systemUnderTest.Database.EnsureCreated();
    }

    [Fact]
    public async Task Tenants_ReturnsAllTenants()
    {
        await InitializeSampleTenants();

        systemUnderTest.Tenants
            .Should().HaveCount(2)
            .And.ContainSingle(tenant => tenant.Id == 1)
            .And.ContainSingle(tenant => tenant.Id == 2);
    }

    [Fact]
    public async Task Customers_ReturnsCustomersFromAllTenants()
    {
        await InitializeSampleCustomers();

        systemUnderTest.Customers
            .Should().HaveCount(4)
            .And.Contain(customer => customer.TenantId == 1)
            .And.Contain(customer => customer.TenantId == 2);
    }

    [Fact]
    public async Task Orders_ReturnsOrdersFromAllTenants()
    {
        await InitializeSampleOrders();

        systemUnderTest.Orders
            .Should().HaveCount(8)
            .And.Contain(order => order.TenantId == 1)
            .And.Contain(order => order.TenantId == 2);
    }

    // [Fact]
    // public async Task SaveChanges_SoftDeletesTenantsInsteadOfRemoving()
    // {
    //     await InitializeSampleTenants();

    //     var tenant = systemUnderTest.Tenants.First();
    //     systemUnderTest.Tenants.Remove(tenant);
    //     await systemUnderTest.SaveChangesAsync();

    //     using QueryFilterContext context = CreateContext();
    //     context.Tenants.Should().ContainSingle();
    //     context.Tenants.IgnoreQueryFilters().Should().HaveCount(2);
    // }

    // [Fact]
    // public async Task SaveChanges_SoftDeletesCustomersInsteadOfRemoving()
    // {
    //     await InitializeSampleCustomers();

    //     var customer = systemUnderTest.Customers.First();
    //     systemUnderTest.Customers.Remove(customer);
    //     await systemUnderTest.SaveChangesAsync();

    //     using QueryFilterContext context = CreateContext();
    //     context.Customers.Should().HaveCount(3);
    //     context.Customers.IgnoreQueryFilters().Should().HaveCount(4);
    // }

    // [Fact]
    // public async Task SaveChanges_SoftDeletesOrdersInsteadOfRemoving()
    // {
    //     await InitializeSampleOrders();

    //     var order = systemUnderTest.Orders.First();
    //     systemUnderTest.Orders.Remove(order);
    //     await systemUnderTest.SaveChangesAsync();

    //     using var context = CreateContext();
    //     context.Orders.Should().HaveCount(7);
    //     context.Orders.IgnoreQueryFilters().Should().HaveCount(8);
    // }

    // [Fact]
    // public async Task Tenants_ReturnsCurrentTenant()
    // {
    //     await InitializeSampleTenants();

    //     systemUnderTest.Tenants
    //         .Should().ContainSingle()
    //         .And.OnlyContain(tenant => tenant.Id == TenantId);
    // }

    // [Fact]
    // public async Task Customers_ReturnsCustomersFromCurrentTenant()
    // {
    //     await InitializeSampleCustomers();

    //     systemUnderTest.Customers
    //         .Should().HaveCount(2)
    //         .And.OnlyContain(customer => customer.TenantId == TenantId);
    // }

    // [Fact]
    // public async Task Orders_ReturnsOrdersFromCurrentTenant()
    // {
    //     await InitializeSampleOrders();

    //     systemUnderTest.Orders
    //         .Should().HaveCount(4)
    //         .And.OnlyContain(order => order.TenantId == TenantId);
    // }

    // [Fact]
    // public async Task SaveChanges_SoftDeletesTenantInsteadOfRemoving()
    // {
    //     await InitializeSampleTenants();

    //     var tenant = systemUnderTest.Tenants.First();
    //     systemUnderTest.Tenants.Remove(tenant);
    //     await systemUnderTest.SaveChangesAsync();

    //     using QueryFilterContext context = CreateContext();
    //     context.Tenants.Should().BeEmpty();
    //     context.Tenants.IgnoreQueryFilters().Where(tenant => tenant.Id == TenantId).Should().ContainSingle();
    // }

    // [Fact]
    // public async Task SaveChanges_SoftDeletesCustomersInsteadOfRemoving()
    // {
    //     await InitializeSampleCustomers();

    //     var customer = systemUnderTest.Customers.First();
    //     systemUnderTest.Customers.Remove(customer);
    //     await systemUnderTest.SaveChangesAsync();

    //     using QueryFilterContext context = CreateContext();
    //     context.Customers.Should().ContainSingle();
    //     context.Customers.IgnoreQueryFilters().Where(customer => customer.TenantId == TenantId).Should().HaveCount(2);
    // }

    // [Fact]
    // public async Task SaveChanges_SoftDeletesOrdersInsteadOfRemoving()
    // {
    //     await InitializeSampleOrders();

    //     var order = systemUnderTest.Orders.First();
    //     systemUnderTest.Orders.Remove(order);
    //     await systemUnderTest.SaveChangesAsync();

    //     using var context = CreateContext();
    //     context.Orders.Should().HaveCount(3);
    //     context.Orders.IgnoreQueryFilters().Where(order => order.TenantId == TenantId).Should().HaveCount(4);
    // }

    // [Fact]
    // public async Task Query_DoesNotCacheTenantIdFilter()
    // {
    //     await InitializeSampleCustomers();
    //     const int ExpectedTenantId = TenantId + 1;
    //     QueryFilterContext context = CreateContext(ExpectedTenantId);

    //     context.Customers.Should().HaveCount(2)
    //         .And.OnlyContain(customer => customer.TenantId == ExpectedTenantId);
    // }

    private Task InitializeSampleCustomers() => InitializeSampleData(Seed.WithSampleCustomers);

    private Task InitializeSampleOrders() => InitializeSampleData(Seed.WithSampleOrders);

    private Task InitializeSampleTenants() => InitializeSampleData(Seed.WithSampleTenants);

    private QueryFilterContext CreateContext(int tenantId = TenantId) => 
        new QueryFilterContext(inMemory.Options
            // , tenantId
            );

    private Task InitializeSampleData(Func<QueryFilterContext, Task> seed)
    {
        using var context = CreateContext();
        return seed(context);
    }

    public void Dispose() => inMemory.Dispose();
}