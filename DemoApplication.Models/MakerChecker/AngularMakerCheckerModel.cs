using System;
using DemoApplication.Enums;

namespace DemoApplication.Models;

public class AngularMakerCheckerModel
{
    public Guid Id { get; set; }
    public MakerCheckerActionsEnum Action { get; set; }
    public Guid MakerUser { get; set; }
    public string MakerUserString { get; set; }
    public Guid CheckerUser { get; set; }
    public string CheckerUserString { get; set; }
    public bool Accepted { get; set; }
    public DateTimeOffset MakerDate { get; set; }
    public DateTimeOffset? CheckerDate { get; set; }
    public string Motivation { get; set; }
    public Guid[] Files { get; set; }
    public string[] FileNames { get; set; }
    public string Model { get; set; }
    public string Context { get; set; }
}
