using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ColorSorting3D.LevelEditor
{
    [Serializable]
    internal class Event : IComparable<Event>, IComparable<EventID>
    {
        public EventID ID;
        public UnityEvent Listeners;

        public Event(EventID ID, params UnityAction[] callbacks)
        {
            this.ID = ID;
            Listeners = new();

            foreach (var callback in callbacks)
                Listeners.AddListener(callback);
        }

        public int CompareTo(Event other)
            => ID.CompareTo(other.ID);

        public int CompareTo(EventID other)
            => ID.CompareTo(other);
    }

    internal class EventManager : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField] private List<Event> Events;

        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize()
        {
            Events.Sort();
        }

        private bool TryGetEvent(EventID id, out int resultIndex)
        {
            resultIndex = -1;
            int minIndex = 0;
            int maxIndex = Events.Count - 1;

            while (minIndex <= maxIndex)
            {
                int midIndex = (minIndex + maxIndex) / 2;
                int comparisonResult = Events[midIndex].CompareTo(id);

                if (comparisonResult == 0)
                {
                    resultIndex = midIndex;
                    return true;
                }

                if (comparisonResult > -1)
                    maxIndex = midIndex - 1;
                else
                    minIndex = midIndex + 1;
            }

            return false;
        }

        public void AddListener(EventID id, UnityAction callback)
        {
            if (TryGetEvent(id, out var resultIndex))
            {
                Events[resultIndex].Listeners.AddListener(callback);
                return;
            }

            var newEvent = new Event(id, callback);
            Events.Add(newEvent);
            Events.Sort();
        }

        public void RemoveListener(EventID id, UnityAction callback)
        {
            if (!TryGetEvent(id, out var resultIndex))
            {
                Debug.Log($"Cant find EventID: {id}");
                return;
            }

            var resultEvent = Events[resultIndex];
            resultEvent.Listeners.RemoveListener(callback);

            if (resultEvent.Listeners.GetPersistentEventCount() == 0)
                Events.Remove(resultEvent);
        }

        public void PostEvent(EventID id)
        {
            if (!TryGetEvent(id, out var resultIndex))
            {
                Debug.Log($"Cant find EventID: {id}");
                return;
            }

            Events[resultIndex].Listeners.Invoke();
        }

        public void PostEvent(string idStr)
        {
            var id = (EventID)Enum.Parse(typeof(EventID), idStr);

            if (!TryGetEvent(id, out var resultIndex))
            {
                Debug.Log($"Cant find EventID: {id}");
                return;
            }

            Events[resultIndex].Listeners.Invoke();
        }
    }
}    