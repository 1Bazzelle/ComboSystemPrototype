using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlash : MonoBehaviour
{
    [ColorUsage(true, true)]
    private Color flashColor = Color.white;

    private SpriteRenderer[] renderers;
    private Material[] materials;

    private bool flashQueued;
    private float duration;

    private float timeElapsed;

    void OnEnable()
    {
        renderers = GetComponentsInChildren<SpriteRenderer>();

        materials = new Material[renderers.Length];
        for(int i = 0; i < materials.Length; i++)
        {
            materials[i] = renderers[i].material;
        }
    }
    void Update()
    {
        if(flashQueued)
        {
            if(duration > timeElapsed)
            {
                float flashIntensity = Mathf.Lerp(1, 0, timeElapsed / duration);

                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i].SetFloat("_FlashIntensity", flashIntensity);
                }
            }
            if(duration < timeElapsed)
            {
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i].SetFloat("_FlashIntensity", 0);
                }
            }

            timeElapsed += Time.deltaTime;
        }
    }
    
    public void QueueFlash(float dur, Color flashColor_)
    {
        duration = dur;
        flashQueued = true;

        timeElapsed = 0;

        for(int i = 0; i < materials.Length; i++) 
        {
            materials[i].SetColor("_FlashColor", flashColor);
        }
    }

    public void UnFlash()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetFloat("_FlashIntensity", 0);
        }

        flashQueued = false;
    }
}
