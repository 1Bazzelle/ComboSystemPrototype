using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeFrame : MonoBehaviour
{
    private bool timeStopped = false;
    public void FreezeTime(AttackNode attack)
    {
        if (timeStopped) return;

        timeStopped = true;

        Time.timeScale = 0;
        StartCoroutine(Wait(attack.freezeFrameTime));    
    }

    private IEnumerator Wait(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1;
        timeStopped = false;
    }
}
