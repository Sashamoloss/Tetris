using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameEvent", menuName = "GameSO/Events/GameEvent")]
public class GameEventSO : ScriptableObject
{

    private List<GameEventListener> listeners = new List<GameEventListener>();
    public void RegisterListener(GameEventListener listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(GameEventListener listener)
    {
        listeners.Remove(listener);
    }

    public void Raise()
    {
        if (name== "ChuteInstantEvent")
        {
            Debug.Log("slt");
        }
        for (int i = listeners.Count - 1; i >= 0; --i)
        {
            listeners[i].RaiseEvent();
        }
    }
}
