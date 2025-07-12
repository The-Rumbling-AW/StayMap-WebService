namespace backendAppsWeb.Communities.Domain.Model.ValueObjects;

public record Tags
{
    public List<string> Value { get; private set; }

    public Tags(List<string> tags)
    {
        Value = tags ?? new List<string>();
    }
}
