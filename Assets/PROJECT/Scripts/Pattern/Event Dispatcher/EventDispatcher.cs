using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuocAnh.pattern;
public class EventDispatcher : Singleton<EventDispatcher>
{
    private Dictionary<EventID, Action<object>> _events = new Dictionary<EventID, Action<object>>();


    // Register to listen for eventID, callback will be invoke when event with eventID be raise
    public void RegisterListener(EventID eventID, Action<object> callback)
    {
        if (_events.ContainsKey(eventID))
        {
            _events[eventID] += callback;
        }
        else
        {
            _events.Add(eventID, callback);
        }

    }

    // Post event, this will notify all listener which register to listen for eventID
    public void FireEvent(EventID eventID, object param = null)
    {
        if (_events.ContainsKey(eventID))
        {
            var _action = _events[eventID];

            if (_action != null)
            {
                _action.Invoke(param);
            }
        }
        else
        {
            RemoveListener(eventID, null);
        }

    }

    // Use for Unregister, not listen for an event anymore.
    public void RemoveListener(EventID eventID, Action<object> callback)
    {
        if (_events.ContainsKey(eventID))
        {
            _events[eventID] -= callback;
        }

    }
}

/// An Extension class, declare some "shortcut" for using EventDispatcher
public static class EventDispatcherExtension
{
    /// Use for registering with EventDispatcher
    public static void RegisterListener(EventID eventID, Action<object> callback)
    {
        EventDispatcher.Instance.RegisterListener(eventID, callback);
    }

    /// Post event with param
    public static void FireEvent(EventID eventID, object param)
    {
        EventDispatcher.Instance.FireEvent(eventID, param);
    }

    /// Post event with no param (param = null)
    public static void FireEvent(EventID eventID)
    {
        EventDispatcher.Instance.FireEvent(eventID, null);
    }
}
