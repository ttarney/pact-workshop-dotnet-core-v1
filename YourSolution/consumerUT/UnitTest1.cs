using Consumer;
using consumerUT.Support;
using PactNet.Mocks.MockHttpService;
using System;
using System.Collections.Generic;
using Xunit;
using Shouldly;

namespace consumerUT
{
    public class ConsumerPactTests : IClassFixture<ConsumerPactClassFixture>
    {
        private IMockProviderService _mockProviderService;
        private string _mockProviderServiceBaseUri;

        public ConsumerPactTests(ConsumerPactClassFixture fixture)
        {
            _mockProviderService = fixture.MockProviderService;
            _mockProviderService.ClearInteractions();
            _mockProviderServiceBaseUri = fixture.MOckProviderServiceBaseUri;
        }

        [Fact]
        public void provider_should_handle_valid_date_parameter()
        {
            string invalidRequestMessage = "validDateTime is not a date or time";
            _mockProviderService.Given("There is data")
                .UponReceiving("An invalid GET request for Date Validation with invalid date parameter")
                .With
                    (
                        new PactNet.Mocks.MockHttpService.Models.ProviderServiceRequest
                        {
                            Method = PactNet.Mocks.MockHttpService.Models.HttpVerb.Get,
                            Path = "/api/provider",
                            Query = "validDateTime=lolz"
                        }
                    )
                    .WillRespondWith
                    (
                        new PactNet.Mocks.MockHttpService.Models.ProviderServiceResponse
                        {
                            Status = 400,
                            Headers = new Dictionary<string, object>
                            {
                                { "Content-TYpe", "application/json; charset=utf-8" }
                            },
                            Body = new
                            {
                                message = invalidRequestMessage
                            }
                        });

            var result = ConsumerApiClient.ValidateDateTimeUsingProviderApi("lolz", _mockProviderServiceBaseUri).GetAwaiter().GetResult();
            var resultBodyText = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            resultBodyText.ShouldContain(invalidRequestMessage);
            // Assert
            //Assert.Contains(invalidRequestMessage, resultBodyText);
        }
    }

}
