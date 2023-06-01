using UnityEngine;
using Event = EventSystem.Event;

public class TextSequenceCompleteEvent : Event
{
    public string CompletedSequenceName;
    public TextSequenceCompleteEvent(string completedSequenceName, string description) : base(description)
    {
        CompletedSequenceName = completedSequenceName;
    }
}