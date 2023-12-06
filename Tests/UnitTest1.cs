using Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace TestProject1;

public class Tests
{
    private IServiceProvider _serviceProvider;
    
    [SetUp]
    public void Setup()
    {
        
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }
}