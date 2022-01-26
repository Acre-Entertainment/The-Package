using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GeneralEvent : MonoBehaviour
{
    public UnityEvent[] eventTrigger;
    public EventInfo[] events;

    public void PlayEventTrigger(int eventIndex)
    {
        eventTrigger[eventIndex].Invoke();
    }

    public void PlayEvent(string eventID)
    {
        UnityEvent eventToPlay = FindEvent(eventID);

        if (eventToPlay != null)
            eventToPlay.Invoke();
    }

    UnityEvent FindEvent(string eventID)
    {
        foreach(EventInfo eventInfo in events)
        {
            if(eventInfo.eventId == eventID)
                return eventInfo.eventTrigger;
        }

        Debug.Log($"Tried to play {eventID} event, but it does not exists");
        return null;
    }
}

[System.Serializable]
public class EventInfo
{
    public string eventId;
    public UnityEvent eventTrigger;
}