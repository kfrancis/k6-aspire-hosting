namespace K6.Hosting.Core;

public class K6Options
{
    public string ScriptDirectory { get; set; } = string.Empty;
    public string ScriptFileName { get; set; } = string.Empty;
    public string ImageRegistry { get; set; }
    public string ImageName { get; set; }
    public string ImageTag { get; set; }
}