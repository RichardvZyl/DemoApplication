using System;
using DemoApplication.Enums;

namespace DemoApplication.Models;

public class NewMakerCheckerModel
{
    public Guid Id { get; set; }
    public MakerCheckerActionsEnum Action { get; set; }
    public string Motivation { get; set; } = string.Empty;
    public Guid[] Files { get; set; } = Array.Empty<Guid>();
    public string Model { get; set; } = string.Empty;
}
