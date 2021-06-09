using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    // The game event instance to register to.
    public GameEventSO GameEvent;
    // The unity event responce created for the event.
    public UnityEvent Response;

    private void OnEnable()
    {
        GameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        GameEvent.UnregisterListener(this);
    }

    public void RaiseEvent()
    {
        Response.Invoke();
    }
}
