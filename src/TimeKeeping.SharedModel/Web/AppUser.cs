namespace Th11s.TimeKeeping.SharedModel.Web
{
    public record AppUser(
        string Anzeigename,
        List<(string Type, string Value)> Claims
        );
}
