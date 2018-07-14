
using System.Collections.Generic;

using Utilities;

namespace EventSystem
{
    public class ObserverList
    {
        private bool lockOnNotify = false;
        private Queue<EventBroadcaster.EventListenerDelegate> addQueue;
        private Queue<EventBroadcaster.EventListenerDelegate> removeQueue;

        private string eventName = "";
        private List<EventBroadcaster.EventListenerDelegate> eventListeners;
        public int Count
        {
            get
            {
                return this.eventListeners == null ? 0 : this.eventListeners.Count;
            }
        }

        public ObserverList(string eventName)
        {
            this.eventName = eventName;
        }

        public bool Contains(EventBroadcaster.EventListenerDelegate callback)
        {
            return this.eventListeners != null && this.eventListeners.Contains(callback);
        }

        public bool AddObserver(EventBroadcaster.EventListenerDelegate callback)
        {
            if (this.lockOnNotify)
            {
                if (this.addQueue == null)
                {
                    this.addQueue = new Queue<EventBroadcaster.EventListenerDelegate>();
                }

                this.addQueue.Enqueue(callback);
                return true;
            }

            if (this.eventListeners == null)
            {
                this.eventListeners = new List<EventBroadcaster.EventListenerDelegate>();
            }

            if (this.eventListeners.Contains(callback))
            {
                UnityEngine.Debug.LogWarning("Callback for observer already exists.");
                return false;
            }

            this.eventListeners.Add(callback);
            return true;
        }

        public bool RemoveObserver(EventBroadcaster.EventListenerDelegate callback)
        {
            if (this.lockOnNotify)
            {
                if (this.removeQueue == null)
                {
                    this.removeQueue = new Queue<EventBroadcaster.EventListenerDelegate>();
                }

                this.removeQueue.Enqueue(callback);
                return false;
            }

            if (this.eventListeners == null)
            {
                return false;
            }

            bool removed = this.eventListeners.Remove(callback);
            return removed;
        }

        public void NotifyObserver(Parameters p)
        {
            if (this.eventListeners == null)
            {
                return;
            }

            this.lockOnNotify = true;

            int eventLength = eventListeners.Count;
            for (int i = eventLength - 1; i >= 0; i--)
            {
                EventBroadcaster.EventListenerDelegate callback = eventListeners[i];
                if (callback != null)
                {
                    callback(p);
                }
            }

            this.lockOnNotify = false;
            while (this.addQueue != null && this.addQueue.Count > 0)
            {
                EventBroadcaster.EventListenerDelegate toAdd = this.addQueue.Dequeue();
                this.AddObserver(toAdd);
            }

            while (this.removeQueue != null && this.removeQueue.Count > 0)
            {
                EventBroadcaster.EventListenerDelegate toRemove = this.removeQueue.Dequeue();
                this.RemoveObserver(toRemove);
            }

            this.addQueue = this.removeQueue = null;
        }
    }
}
