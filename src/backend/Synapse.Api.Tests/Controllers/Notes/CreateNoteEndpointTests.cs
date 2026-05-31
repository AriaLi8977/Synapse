namespace Synapse.Api.Tests.Controllers.Notes;

public class CreateNoteEndpointTests
{

    [Fact]
    public async Task Root_Endpoint_Should_Return_Ok()
    {
        using var factory =
            new WebApplicationFactory<Program>();

        var client = factory.CreateClient();

        var response = await client.GetAsync("/");

        response.StatusCode.Should()
            .Be(HttpStatusCode.OK);

        var content =
            await response.Content.ReadAsStringAsync();

        content.Should()
            .Contain("Synapse API is running");
    }

    // [Fact]
    // public async Task CreateNote_Should_Return_Success()
    // {
    //     // Arrange
    //     using var factory = new WebApplicationFactory<Program>();
    //     var client = factory.CreateClient();
    //     var request = new
    //     {
    //         content = "Integration Test Note"
    //     };

    //     // Act
    //     var response = await client.PostAsJsonAsync( "/api/notes", request);

    //     // Assert
    //     response.StatusCode.Should().Be(HttpStatusCode.OK);
    // }
}