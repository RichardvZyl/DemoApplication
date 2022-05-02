using System;
using DemoApplication.Enums;

namespace DemoApplication.Models;

public class AngularMakerCheckerModel
{
    public Guid Id { get; set; }
    public MakerCheckerActionsEnum Action { get; set; }
    public Guid MakerUser { get; set; }
    public string MakerUserString { get; set; } = string.Empty;
    public Guid CheckerUser { get; set; }
    public string CheckerUserString { get; set; } = string.Empty;
    public bool Accepted { get; set; }
    public DateTimeOffset MakerDate { get; set; }
    public DateTimeOffset? CheckerDate { get; set; }
    public string Motivation { get; set; } = string.Empty;
    public Guid[] Files { get; set; } = Array.Empty<Guid>();
    public string[] FileNames { get; set; } = Array.Empty<string>();
    public string Model { get; set; } = string.Empty;
    public string Context { get; set; } = string.Empty;
}
