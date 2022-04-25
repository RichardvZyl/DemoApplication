using DemoApplication.Domain;
using DemoApplication.Enums;
using DemoApplication.Models;

namespace DemoApplication.Factory;

public static class MakerCheckerFactory
{
    public static MakerChecker Create(NewMakerCheckerModel model, Guid activeUserId, MakerCheckerActionsEnum action)
        => new(
            action,
            activeUserId,
            Guid.Empty,
            DateTimeOffset.UtcNow,
            new DateTimeOffset?(),
            false,
            model.Motivation,
            model.Files,
            model.Model
        );
}
