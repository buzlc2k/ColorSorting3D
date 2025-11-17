using System;
using System.Collections.Generic;

namespace Butserver{
    public class Observer<T_ID>
    {
        private static Dictionary<T_ID, List<Action<object>>> Events = new();
        
        public static void AddListener(T_ID eventID, Action<object> callback)
        {
            if (!Events.TryGetValue(eventID, out var eventList))
            {
                eventList = new List<Action<object>>();
                Events[eventID] = eventList;
            }
            eventList.Add(callback);
        }
        
        public static void RemoveListener(T_ID eventID, Action<object> callback)
        {
            if (!Events.TryGetValue(eventID, out var eventList)) return;
            eventList.Remove(callback);
            
            if (eventList.Count == 0)
            {
                Events.Remove(eventID);
            }
        }
        
        public static void PostEvent(T_ID eventID, object param)
        {
            if (!Events.TryGetValue(eventID, out var eventList)) return;
            foreach (var callback in eventList)
            {
                callback(param);
            }
        }
        
        public static void ClearAllListeners()
        {
            Events.Clear();
        }
    }
}