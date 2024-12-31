using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackEventListener : MonoBehaviour
{
    [SerializeField] private AttackEvent gameEvent;
    [SerializeField] private UnityEvent<AttackNode> response;
    public void OnEventOccured(AttackNode attack)
    {
        response.Invoke(attack);
    }

    private void OnEnable()
    {
        gameEvent.Register(this);
    }
    private void OnDisable()
    {
        gameEvent.Unregister(this);
    }
}
