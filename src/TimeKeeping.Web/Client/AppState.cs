namespace TimeKeeping.Web.Client;

public class AppState
{
    private Guid _arbeitsplatzId;
    public Guid ArbeitsplatzId
    {
        get => _arbeitsplatzId;
        set
        {
            _arbeitsplatzId = value;
            AppStateChanged?.Invoke();
        }
    }

    public event Action? AppStateChanged;

}
