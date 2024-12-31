using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static Vector2 InterpolationEaseIn(Vector2 startPos, Vector2 endPos, float t, float amplify)
    {
        float easeInT = Mathf.Pow(t, amplify * 2); // Ease In curve
        return Vector2.Lerp(startPos, endPos, easeInT);
    }
    public static Vector2 InterpolationEaseOut(Vector2 startPos, Vector2 endPos, float t, float amplify)
    {
        float easeOutT = 1f - Mathf.Pow(1f - t, 2f * amplify); // Modified Ease Out curve with amplify
        return Vector2.Lerp(startPos, endPos, easeOutT);
    }
    public static Vector2 InterpolationEaseInOut(Vector2 startPos, Vector2 endPos, float t, float amplify)
    {
        float easeInOutT;

        if (t < 0.5f)
        {
            easeInOutT = 4f * Mathf.Pow(t, 3f * amplify);
        }
        else
        {
            easeInOutT = 1f - Mathf.Pow(-2f * t + 2f, 3f * amplify) / 2f;
        }

        return Vector2.Lerp(startPos, endPos, easeInOutT);
    }
}
