using System;
using System.Collections.Generic;
using UnityEngine;

namespace EventSystem
{
    /**
     * <summary>Handles registering, unregistering and invocation of Events</summary>
     * <remarks>This is a singleton class. There should only ever be one of these present in a scene</remarks>
     */
    public class EventHandler : MonoBehaviour
    {
        private static EventHandler _instance;

        private delegate void EventListener(Event info);

        private Dictionary<Type, List<EventListener>> listenerRegistry = new Dictionary<Type, List<EventListener>>();

        public static EventHandler instance { 
            get 
            { 
                if (_instance == null)
                {
                    _instance = FindObjectOfType<EventHandler>();
                    if (_instance == null)
                    {
                        Debug.LogError("EventHandler was not found in the scene, have you added the script to a GameObject?");
                    }
                }
                return _instance; 
            } 
        }

        private void OnEnable()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            
            if (_instance != this)
            {
                Debug.LogError("More than one EventHandler! GameObject: " + gameObject.name);
            }
        }
    
        /**
         * <summary>Register a listener method to a specific type of <c>Event</c> </summary>
         * <remarks>
         *  Registering listeners should be done before anything else when starting the game and whenever the GameObject is activated.
         *  It should therefore be done in the <c>Awake</c> method as well as the <c>OnEnable()</c> method
         * </remarks>
         * <param name="listener">The method that is called when the specified event is invoked. See example for method requirements </param>
         * <typeparam name="TEvent"> The type of event that the listener will listen for </typeparam>
         * <example>
         *  Register a new listener that calls the method <c>OnDeath()</c> when a <c>DieEvent</c> is invoked: <br/><br/>
         *  Create a new listener method that handles the event invocation. It needs to take in a <c>DieEvent</c> as parameter: <br/> 
         * <code>
         *  private void OnDeath(DieEvent eventInfo)
         *  {
         *      //Do something with eventInfo
         *  }
         * </code><br/>
         *  Register the listener in the <c>OnEnable()</c> method: <br/>
         * <code>
         *  EventHandler.instance.RegisterListener&lt;DieEvent&gt;(OnDeath);
         * </code>
         * </example>
         */
        public void RegisterListener<TEvent>(Action<TEvent> listener) where TEvent : Event
        {
            // Get the type of Event
            Type eventType = typeof(TEvent);

            // If the dictionary hasn't been declared yet
            listenerRegistry ??= new Dictionary<Type, List<EventListener>>();

            // If the event hasn't been registered before
            if (!listenerRegistry.ContainsKey(eventType))
            {
                listenerRegistry[eventType] = new List<EventListener>();
            }

            // Wrap listener call to return an EventListener
            void Wrapper(Event eventInfo)
            {
                listener((TEvent)eventInfo);
            }

            // Add listener to the Event
            listenerRegistry[eventType].Add(Wrapper);
        }
        
        /**
         * <summary>Unregister a listener from a specific type of <c>Event</c> </summary>
         * <remarks>
         *  You should always unregister a listener when deactivating, destroying or before loading a new scene to ensure that the invoker is calling the correct listener.
         *  It should therefore be done in the <c>OnDisable()</c> and <c>OnDestroy()</c> methods.
         * </remarks>
         * <param name="listener" > The method that should be unregistered </param>
         * <typeparam name="TEvent"> The type of <c>Event</c> that the <paramref name="listener"/> should be unregistered from </typeparam>
         * <example>
         *  Unregistering <c>OnDeath()</c> from the <c>DieEvent</c>:<br/>
         * <code>
         *  EventHandler.instance.RegisterListener&lt;DieEvent&gt;(OnDeath);
         * </code>
         * </example>
         */
        public void UnregisterListener<TEvent>(Action<TEvent> listener) where TEvent : Event
        {
            // Get the type of event passed in
            Type eventType = typeof(TEvent);

            // If the event is not in the dictionary or the list of listeners is empty, there's nothing to unregister
            if (!listenerRegistry.ContainsKey(eventType) || listenerRegistry[eventType].Count == 0)
            {
                return;
            }

            // Wrap listener call to return an EventListener
            void Wrapper(Event eventInfo)
            {
                listener((TEvent)eventInfo);
            }

            // Remove listener from the list in dictionary
            listenerRegistry[eventType].Remove(Wrapper);
        }
        
        /**
         * <summary>Invoke an <c>Event</c> of a certain type</summary>
         * <remarks>See the <c>Event</c> class to learn about how to create a eventInfo container</remarks>
         * <param name="eventInfo">Information about the event that the listener might need</param>
         * <example>
         *  Invoking a <c>DieEvent</c>: <br/>
         * <code>
         * EventHandler.instance.InvokeEvent(
         *      new DieEvent(
         *      description: "Object " + gameObject.name + " has died",
         *      invoker: gameObject,
         *      deathSounds: deathSounds,
         *      deathParticles: particlePrefab
         *      ));
         * </code>
         * </example>
         */
        public void InvokeEvent(Event eventInfo)
        {
            // Get the type of event
            Type eventClass = eventInfo.GetType();
            
            if (!listenerRegistry.ContainsKey(eventClass))
            {
                // Event has not been added to registry
                Debug.LogError("Event not found in registry, have you registered the listener?");
                return;
            }

            if (listenerRegistry?[eventClass] == null)
            {
                // No one to shout at
                return;
            }

            // Shout at anyone listening
            foreach (EventListener listener in listenerRegistry[eventClass])
            {
                listener(eventInfo);
            }
        }
    }
}
