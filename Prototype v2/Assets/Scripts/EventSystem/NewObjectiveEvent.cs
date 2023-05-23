using UnityEngine;

namespace EventSystem
{
    public class NewObjectiveEvent : Event
    {
        public GameObject newObjective;
        
        public NewObjectiveEvent(GameObject newObjective, string description) : base(description)
        {
            this.newObjective = newObjective;
        }
    }
}
