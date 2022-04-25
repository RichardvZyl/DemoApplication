using System;
using System.Text.Json;
using DemoApplication.Enums;
using DemoApplication.Models;
using Xunit;

namespace DemoApplication.Framework.tests;

public class UnitTestMakerCheckerService
{
    private readonly IMakerCheckerService _makerCheckerService;

    public UnitTestMakerCheckerService
    (
        IMakerCheckerService makerCheckerService
    ) => _makerCheckerService = makerCheckerService;

    [Fact]
    public void CreateNewUserTest()
    {// This is the only maker checker action you cannot perform directly (without maker checker)
        var result = _makerCheckerService.AddAsync(makerChecker, Guid.NewGuid());

        Assert.True(result.IsCompletedSuccessfully, "Maker Checker did not succeed");
    }

    private static readonly NewMakerCheckerModel makerChecker = new()
    {
        Action = MakerCheckerActionsEnum.CreateUser,
        Motivation = "Test",
        Files = Array.Empty<Guid>(),
        Model = JsonSerializer.Serialize
        (
            new UserModel
            {
                Id = Guid.Empty,
                Name = "UnitTestName",
                Surname = "UnitTestSurname",
                Email = "unit@test.com",
                Auth = new AuthModel
                {
                    Login = "unit@test.com",
                    Password = "test",
                    Role = RolesEnum.Clerk
                }
            }
        )
    };
}
