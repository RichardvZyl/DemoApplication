using System;
using System.Collections.Generic;
using DemoApplication.Enums;

namespace DemoApplication.Models;

public class MakerCheckerModel
{
    public Guid Id { get; set; }
    public MakerCheckerActionsEnum Action { get; set; }
    public Guid MakerUser { get; set; }
    public Guid CheckerUser { get; set; }
    public bool Accepted { get; set; }
    public DateTimeOffset MakerDate { get; set; }
    public DateTimeOffset? CheckerDate { get; set; }
    public string Motivation { get; set; } = string.Empty;
    public IEnumerable<Guid> Files { get; set; } = Array.Empty<Guid>();
    public string Model { get; set; } = string.Empty;
}
