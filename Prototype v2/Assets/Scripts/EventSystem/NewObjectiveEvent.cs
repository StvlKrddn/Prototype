using UnityEngine;
using Event = EventSystem.Event;

namespace Scrips.EventSystem
{
    /**
     * <summary>Event that is invoked when a new objective is reached</summary>
     */
    public class NewObjectiveEvent : Event
    {
        public readonly GameObject NewObjectiveNode;
        
        public NewObjectiveEvent(GameObject newObjectiveNode, string description) : base(description)
        {
            NewObjectiveNode = newObjectiveNode;
        }
    }
}
