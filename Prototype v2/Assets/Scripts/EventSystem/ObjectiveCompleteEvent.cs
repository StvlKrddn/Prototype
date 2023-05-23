using UnityEngine;
using Event = EventSystem.Event;

namespace EventSystem
{
    /**
     * <summary>Event that is invoked when an objective is reached</summary>
     */
    public class ObjectiveCompleteEvent : Event
    {
        public readonly GameObject completedObjective;
        
        public ObjectiveCompleteEvent(GameObject completedObjective, string description) : base(description)
        {
            this.completedObjective = completedObjective;
        }
    }
}
