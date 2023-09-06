namespace Th11s.TimeKeeping.Data.Entities
{
    public record Nachverfolgungseintrag(
        DateTimeOffset At,
        string Command,
        string Data,

        int UserId,
        string UserName
        );
    }
}
