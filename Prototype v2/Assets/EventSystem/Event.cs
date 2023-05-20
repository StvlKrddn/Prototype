namespace EventSystem
{
    // Essentials 
    /**
     * <summary>Base class of all types of <c>Event</c></summary>
     * <remarks>To create a new type of <c>Event</c>, simply create a new class that extends this one</remarks>
     */
    public abstract class Event
    {
        public readonly string Description;
        
        /**
         * <summary>The base constructor for an <c>Event</c></summary>
         * <param name="description"> A description of of the event. Could be why the event was invoked for example.</param>
         */
        protected Event(string description)
        {
            Description = description;
        }
    }
}