using System.Collections.Generic;
using Abstractions.Domain;

namespace DemoApplication.Domain;

public sealed class FullName : ValueObject
{
    public FullName(string name, string surname)
    {
        Name = name;
        Surname = surname;
    }

    public string Name { get; init; }

    public string Surname { get; init; }

    protected override IEnumerable<object> GetEquals()
    {
        yield return Name;
        yield return Surname;
    }
}
