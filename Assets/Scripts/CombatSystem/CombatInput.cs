using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Input", menuName = "New Input")]
[System.Serializable]
public class CombatInput : ScriptableObject
{
    public enum SecondaryInput
    {
        None,
        Forward,
        Backward,
        Up,
        Down,
    }

    public SecondaryInput secondary;

    public static KeyCode ConvertToKeyCode(SecondaryInput input, bool playerFacingRight)
    {
        return input switch
        {
            SecondaryInput.Forward => playerFacingRight ? Constants.forward : Constants.backward,
            SecondaryInput.Backward => playerFacingRight ? Constants.backward : Constants.forward,
            SecondaryInput.Up => Constants.up,
            SecondaryInput.Down => Constants.down,
            _ => 0,
        };
    }
}
