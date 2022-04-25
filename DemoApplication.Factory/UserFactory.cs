using DemoApplication.Domain;
using DemoApplication.Models;

namespace DemoApplication.Factory;

public static class UserFactory
{
    public static User Create(UserModel model, Auth auth) 
        => new
        (
            new FullName(model.Name, model.Surname),
            model.Email,
            auth
        );
}
