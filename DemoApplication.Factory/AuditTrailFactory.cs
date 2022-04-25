using DemoApplication.Domain;
using DemoApplication.Models;

namespace DemoApplication.Factory;

public static class AuditTrailFactory
{
    public static AuditTrail Create(AuditTrailModel model)
        => new
           (
               model.Date,
               model.UserId,
               model.DisplayContext,
               model.Model,
               model.Contents
           );
}
