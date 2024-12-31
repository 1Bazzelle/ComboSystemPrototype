using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    // Inputs
    public static KeyCode attackInput = KeyCode.Mouse0;

    public static KeyCode forward = KeyCode.D;
    public static KeyCode backward = KeyCode.A;
    public static KeyCode up = KeyCode.W;
    public static KeyCode down = KeyCode.S;




    public static float generalTimeWindow = 0.5f;
    public static float chaosValue = 0.25f; // Currently used for knockback



    // Tags
    public static string Tag_Enemy = "Enemy";

    // LayerMasks
    public static LayerMask Layer_Entity = 1 << LayerMask.NameToLayer("Entity");
}
