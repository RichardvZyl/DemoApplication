using DemoApplication.Domain;
using DemoApplication.Models;

namespace DemoApplication.Factory;

public static class AuthFactory
{
    public static Auth Create(AuthModel model) 
        => new
           (
                model.Login, 
                model.Password, 
                model.Role
           );
}
