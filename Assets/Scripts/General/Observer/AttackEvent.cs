using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "GameEvent")]
public class AttackEvent : ScriptableObject
{
    private List<AttackEventListener> eventListeners = new();

    public void Register(AttackEventListener listener)
    {
        eventListeners.Add(listener);
    }
    public void Unregister(AttackEventListener listener)
    {
        eventListeners.Remove(listener);
    }

    public void Occured(AttackNode attack)
    {
        foreach (AttackEventListener listener in eventListeners)
        {
            listener.OnEventOccured(attack);
        }
    }
}
