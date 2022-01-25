using System;
using System.Threading.Tasks;

namespace DotnetLearn.QueryFilterDemo.Tests;

using Utility;

public class QueryFilterContextBaseTests : IDisposable
{
    private readonly SqliteInMemoryOptionsProvider inMemory = new ();
    private readonly QueryFilterContext systemUnderTest;

    public QueryFilterContextBaseTests()
    {
        systemUnderTest = new QueryFilterContext(inMemory.Options);
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
            .Should()
            .HaveCount(4)
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
            .And.ContainSingle(order => order.TenantId == 2);
    }

    private Task InitializeSampleCustomers() => InitializeSampleData(Seed.WithSampleCustomers);

    private Task InitializeSampleOrders() => InitializeSampleData(Seed.WithSampleOrders);

    private Task InitializeSampleTenants() => InitializeSampleData(Seed.WithSampleTenants);

    private Task InitializeSampleData(Func<QueryFilterContext, Task> seed)
    {
        using QueryFilterContext context = new(inMemory.Options);
        return seed(context);
    }

    public void Dispose()
    {
        inMemory.Dispose();
    }
}