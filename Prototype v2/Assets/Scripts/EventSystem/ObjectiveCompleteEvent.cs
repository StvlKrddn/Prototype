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
        public readonly GameObject nextObjective;
        
        public ObjectiveCompleteEvent(GameObject completedObjective, GameObject nextObjective, string description) : base(description)
        {
            this.completedObjective = completedObjective;
            this.nextObjective = nextObjective;
        }
    }
}
