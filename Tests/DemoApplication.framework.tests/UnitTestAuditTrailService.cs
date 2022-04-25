using System;
using Xunit;

namespace DemoApplication.Framework.tests
{
    public class UnitTestAuditTrailService
    {
        private readonly IAuditTrailService _auditTrailService;

        public UnitTestAuditTrailService
        (
            IAuditTrailService auditTrailService
        )
        {
            _auditTrailService = auditTrailService;
        }

        [Fact]
        public void CreateTest()
        {
            var result = _auditTrailService.AddAsync("EmptyTestModel", Guid.NewGuid());

            Assert.True(result.IsCompletedSuccessfully, "audit trail did not succeed");
        }

    }
}
