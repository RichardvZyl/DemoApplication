using DemoApplication.Models;
using System;
using Xunit;

namespace DemoApplication.Framework.tests
{
    public class UnitTestEntitlementService
    {
        private readonly IEntitlementExceptionsService _entitlementService;

        public UnitTestEntitlementService
        (
            IEntitlementExceptionsService entitlementService
        )
        {
            _entitlementService = entitlementService;
        }

        [Fact]
        public void CreateTest()
        {
            // Create a Audit Trail Entry and see if it is successful
            var result = _entitlementService.AddAsync(entitlement, "", Guid.NewGuid());

            Assert.True(result.IsCompletedSuccessfully, "transaction did not succeed");
        }

        private static readonly EntitlementExceptionsModel entitlement = new EntitlementExceptionsModel
        {
            ExpiresOn = DateTimeOffset.UtcNow.AddDays(-1), // I don't want the testing data to be valid
            UserId = Guid.NewGuid(),
        };

    }
}
