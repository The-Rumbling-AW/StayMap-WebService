namespace  backendAppsWeb.IAM.Domain.Model.ValueObjects;

public record EmailAddress(string Address)
{
    public EmailAddress() : this(string.Empty) { }
}