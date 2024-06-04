[TestFixture]
public class BaseTests
{
    public HttpClient Client { get; set; }

    [OneTimeSetUp]
    public async Task SetUp()
    {
        Client = ApiFixture.HttpClient;
    }
}

