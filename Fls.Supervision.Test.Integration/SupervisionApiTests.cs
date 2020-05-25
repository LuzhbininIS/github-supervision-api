using System;
using Fls.Supervision.Api;
using Fls.Supervision.Api.Commands;
using Fls.Supervision.Test.Integration.Helpers;
using Xunit;

namespace Fls.Supervision.Test.Integration
{
    public class SupervisionApiTests
    {
        [Fact]
        public async void PingTest()
        {
            var server = TestHelper.CreateTestServer(webHostBuilder => webHostBuilder.UseSupervisionApi());
            TestHelper.EnsureBlankTestDb(server);
            var client = server.CreateTestClient();
            var result = await client.GetAsync(() => TestEndpoints.PingGet);
            Assert.Equal("pong", result);
        }

        [Fact]
        public async void GithubPingTest()
        {
            var server = TestHelper.CreateTestServer(webHostBuilder => webHostBuilder.UseSupervisionApi());
            var client = TestHelper.CreateTestClient(server);
            var testWebHookPayload = new ProcessGithubEvent
            {
                Zen = "Anything added dilutes everything else.",
                HookId = 109948940,
                CreatedAt = DateTime.Now,
                Hook = new ProcessGithubEvent.HookPayload
                {
                    Id = 109948940,
                    Type = "Repository",
                    Name = "web"
                }
            };
            var result = await client.PostJsonAsync(TestEndpoints.WebHookPost, testWebHookPayload);
            System.Console.WriteLine(result);
            Assert.Equal("pong", result);
        }
    }
}