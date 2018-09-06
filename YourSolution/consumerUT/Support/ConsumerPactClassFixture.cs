using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using PactNet;
using PactNet.Mocks.MockHttpService;

namespace consumerUT.Support
{
    public class ConsumerPactClassFixture : IDisposable
    {
        public IPactBuilder PactBuilder { get; private set; }
        public IMockProviderService MockProviderService { get; private set; }

        public int MockServerPort { get { return 9222; } }

        private bool _disposedValue = false;
        public string MOckProviderServiceBaseUri
        {
            get
            {
                return string.Format("http://localhost:{0}", MockServerPort);
            }
        }

        public ConsumerPactClassFixture()
        {
            PactConfig pactConfig = new PactConfig()
            {
                SpecificationVersion = "2.0.0",
                PactDir = @"..\..\..\..\..\pacts",
                LogDir = @".\pact_logs"
            };

            PactBuilder = new PactBuilder(pactConfig);

            PactBuilder.ServiceConsumer("Consumer")
                .HasPactWith("Provider");

            MockProviderService = PactBuilder.MockService(MockServerPort);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // This will save the pact file once finished.
                    PactBuilder.Build();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
    }
}
