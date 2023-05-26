using EventSystem;

public class GamePausedEvent : Event
{
    public bool gamePaused;
    public GamePausedEvent(bool gamePaused, string description) : base(description)
    {
        this.gamePaused = gamePaused;
    }
}