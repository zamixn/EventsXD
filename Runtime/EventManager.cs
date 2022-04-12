using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

namespace FrameworksXD.Events
{
    public static class EventManager
    {
        private static Dictionary<System.Type, Dictionary<object, MethodInfo>> BindedListeners = new Dictionary<Type, Dictionary<object, MethodInfo>>();

        public static void AddListener<T>(Action<T> action) where T : EventBase
        {
            var type = typeof(T);
            var target = action.Target;

            if (!BindedListeners.ContainsKey(type))
                BindedListeners.Add(type, new Dictionary<object, MethodInfo>());

            var listeners = BindedListeners[type];
            if (listeners.ContainsKey(target))
            {
                Debug.LogError($"Can't assign duplicate event listeners on same target: target: {target}, event: {type}");
                return;
            }

            listeners.Add(target, action.Method);
        }

        public static void RemoveListener<T>(Action<T> action) where T : EventBase
        {
            var type = typeof(T);
            if (BindedListeners.ContainsKey(type))
            {
                var listeners = BindedListeners[type];
                if (listeners.ContainsKey(action.Target))
                    listeners.Remove(action.Target);
            }
        }

        public static void Broadcast<T>(T eventData) where T : EventBase
        {
            var type = typeof(T);            
            if (BindedListeners.ContainsKey(type))
            {
                var listeners = BindedListeners[type];
                foreach (var listener in listeners)
                {
                    listener.Value.Invoke(listener.Key, new object[] { eventData });
                }
            }
        }
    }

    public abstract class EventBase
    { 
        
    }
}
