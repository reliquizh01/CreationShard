using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Utilities;

namespace EventFunctionSystem
{
    public class EventBroadcaster : MonoBehaviour
    {
        #region Const, Static, and Singleton
        /// <summary>
        /// Model for event listener type. Parameter param, by default, is set to null 
        /// </summary>
        public delegate void EventListenerDelegate(Parameters param);

        /// <summary>
        /// Maximum events invoked per frame. This is added to avoid stack overflow or spikes from multiple or chained events
        /// </summary>
        public const int MAX_POST_EVENTS_PER_FRAME = 3;

        private static EventBroadcaster instance = null;
        public static EventBroadcaster Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject inst = new GameObject("Event Broadcaster");
                    instance = inst.AddComponent<EventBroadcaster>();
                    instance.Initialize();
                }

                return instance;
            }
        }
        #endregion

        #region Fields and Properties
        private bool isInitialized = false;
        public bool IsInitialize()
        {
            return this.isInitialized;
        }

        /// <summary>
        /// Dictionary of event listeners
        /// </summary>
        private Dictionary<string, ObserverList> eventListeners;

        /// <summary>
        /// Queue of events
        /// </summary>
        private Queue<EventNameParam> eventQueue;
        #endregion

        #region Setup
        public void Initialize()
        {
            if (this.isInitialized)
            {
                return;
            }

            DontDestroyOnLoad(this.gameObject);
            this.eventQueue = new Queue<EventNameParam>();

            this.isInitialized = true;
        }

        void OnDestroy()
        {
            this.isInitialized = false;
            instance = null;
        }
        #endregion

        #region Event Management
        void Update()
        {
            if (this.isInitialized == false || this.eventQueue == null)
            {
                return;
            }

            if (this.eventQueue.Count > 0)
            {
                for (int i = 0; i < MAX_POST_EVENTS_PER_FRAME && i < this.eventQueue.Count; i++)
                {
                    EventNameParam eventQueued = this.eventQueue.Dequeue();
                    this.InvokeEventListeners(eventQueued.eventName, eventQueued.param);
                }
            }
        }

        public bool HasActionAtObserver(string eventName, EventListenerDelegate callback)
        {
            if (string.IsNullOrEmpty(eventName) || callback == null)
            {
                return false;
            }

            if (this.eventListeners == null)
            {
                return false;
            }

            if (this.eventListeners.ContainsKey(eventName))
            {
                return this.eventListeners[eventName].Contains(callback);
            }

            return false;
        }

        public bool HasObserverForEvent(string eventName)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                return false;
            }

            if (this.eventListeners == null)
            {
                return false;
            }

            return this.eventListeners.ContainsKey(eventName) && this.eventListeners[eventName].Count > 0;
        }

        public void AddObserver(string eventName, EventListenerDelegate callback)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                Debug.LogWarning("Ensure that the observer has a valid event name");
                return;
            }

            if (callback == null)
            {
                Debug.LogWarning("Ensure that the observer has a valid callback to invoke");
                return;
            }

            if (this.eventListeners == null)
            {
                this.eventListeners = new Dictionary<string, ObserverList>();
            }

            if (this.eventListeners.ContainsKey(eventName) == false)
            {
                this.eventListeners.Add(eventName, new ObserverList(eventName));
            }

            this.eventListeners[eventName].AddObserver(callback);
        }

        public void RemoveActionAtObserver(string eventName, EventListenerDelegate callback)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                Debug.LogWarning("To remove a listener from event, ensure that the event name is valid");
                return;
            }

            if (callback == null)
            {
                Debug.LogWarning("To remove a listener from event, ensure that the callback you are looking for is valid");
                return;
            }

            if (this.eventListeners == null || this.eventListeners.ContainsKey(eventName) == false)
            {
                return;
            }

            this.eventListeners[eventName].RemoveObserver(callback);
            if (this.eventListeners[eventName].Count == 0)
            {
                // After removing a callback, remove the event name if it is empty
                RemoveObserver(eventName);
            }
        }

        public void RemoveObserver(string eventName)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                Debug.LogWarning("To remove a listener from event, ensure that the event name is valid");
                return;
            }

            if (this.eventListeners == null || this.eventListeners.ContainsKey(eventName) == false)
            {
                return;
            }

            // Cleans the event listener from all observers. This is needed to help avoid leaks.
            this.eventListeners.Remove(eventName);
        }

        public void PostEvent(string eventName, Parameters param = null)
        {
            //Don't queue the event unless necessary
            if (this.eventListeners == null || this.eventListeners.ContainsKey(eventName) == false || this.eventListeners[eventName].Count == 0)
            {
                return;
            }

            EventNameParam eventToQueue = new EventNameParam();
            eventToQueue.eventName = eventName;
            eventToQueue.param = param;
            this.eventQueue.Enqueue(eventToQueue);
        }

        private void InvokeEventListeners(string eventName, Parameters param)
        {
            if (this.eventListeners == null || this.eventListeners.ContainsKey(eventName) == false)
            {
                return;
            }

            if (param == null)
            {
                param = new Parameters();
            }

            param.AddParameter<string>("__event_name__", eventName);

            this.eventListeners[eventName].NotifyObserver(param);
        }
        #endregion
    }

    public class EventNameParam
    {
        public string eventName;
        public Parameters param;
    }
}
