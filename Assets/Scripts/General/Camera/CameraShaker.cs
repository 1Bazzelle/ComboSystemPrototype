using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    private Vector3 originalPos;
    private ShakeType currentlyExecuting;

    private float timeElapsed;

    public enum ShakeType
    {
        None,
        ExtraSmall,
        Small,
        Medium,
        Large
    }
    private void Awake()
    {
        originalPos = transform.localPosition;
    }

    void Update()
    {
        if (currentlyExecuting == ShakeType.None) return;

        float duration;
        if (currentlyExecuting > ShakeType.Small) duration = 0.33f;
        else duration = 0.25f;

        ExecuteShake(timeElapsed / duration);

        timeElapsed += Time.deltaTime;
    }

    private void ExecuteShake(float progress)
    {
        if(progress > 1)
        {
            UnShake();
            timeElapsed = 0;
            currentlyExecuting = ShakeType.None;

            return;
        }

        Mathf.Clamp(progress, 0, 1);

        float intensity;

        float chaos;

        switch (currentlyExecuting)
        {
            case ShakeType.ExtraSmall:

                intensity = 0.05f;
                chaos = 0.025f;

                transform.localPosition = new(originalPos.x, originalPos.y + (intensity + Random.Range(-chaos, chaos)) * (1 - progress) * Mathf.Sin(Mathf.PI + progress * 6 * Mathf.PI));

                break;
            case ShakeType.Small:

                intensity = 0.1f;
                chaos = 0.05f;

                transform.localPosition = new(originalPos.x, originalPos.y + (intensity + Random.Range(-chaos,chaos)) * (1 - progress) * Mathf.Sin(Mathf.PI + progress * 6 * Mathf.PI));

                transform.localPosition = new(
                    originalPos.x + ((intensity + Random.Range(-chaos, chaos)) * (1 - progress) * Mathf.Sin(Mathf.PI + progress * 6 * Mathf.PI)/2),
                    originalPos.y + (intensity + Random.Range(-chaos, chaos)) * (1 - progress) * Mathf.Sin(Mathf.PI + progress * 6 * Mathf.PI)
                    );

                break;
            case ShakeType.Medium:

                intensity = 0.15f;
                chaos = 0.1f;

                transform.localPosition = new(
                    originalPos.x + (intensity + Random.Range(-chaos, chaos)) * (1 - progress) * Mathf.Sin(Mathf.PI + progress * 6 * Mathf.PI),
                    originalPos.y + (intensity + Random.Range(-chaos, chaos)) * (1 - progress) * Mathf.Cos(Mathf.PI + progress * 6 * Mathf.PI)
                    );

                break;
            case ShakeType.Large:

                intensity = 0.33f;
                chaos = 0.33f;

                transform.localPosition = new(
                    originalPos.x + (intensity + Random.Range(-chaos, chaos)) * (1 - progress) * Mathf.Sin(Mathf.PI + progress * 6 * Mathf.PI), 
                    originalPos.y + (intensity + Random.Range(-chaos, chaos)) * (1 - progress) * Mathf.Cos(Mathf.PI + progress * 6 * Mathf.PI)
                    );
                break;
        }
    }
    private void UnShake()
    {
        transform.localPosition = originalPos;
    }

    public void QueueSmallShake()
    {
        if (ShakeType.Small > currentlyExecuting)
        {
            UnShake();
            timeElapsed = 0;
            currentlyExecuting = ShakeType.Small;
        }
    }
}