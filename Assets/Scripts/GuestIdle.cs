using UnityEngine;
using System.Collections;

public class GuestIdle : MonoBehaviour
{
    public Sprite[] idleFrames; // size 2 (or more)

    [Header("Timing")]
    public float baseFrameTime = 0.5f;

    [Header("Uneven Timing")]
    [Range(0.5f, 2f)]
    public float[] frameTimeMultipliers = new float[] { 1f, 1.3f };

    [Header("Pause Behavior")]
    public int minCyclesBeforePause = 3;
    public int maxCyclesBeforePause = 8;
    public float pauseDurationMin = 0.8f;
    public float pauseDurationMax = 2.0f;

    private SpriteRenderer sr;
    private int index = 0;
    private int cycleCount = 0;
    private int nextPauseCycle;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        ResetPauseCycle();
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }
    public void StartIdle()
    {
        StartCoroutine(IdleLoop());
    }

    IEnumerator IdleLoop()
    {
        while (true)
        {
            sr.sprite = idleFrames[index];

            // Uneven timing only (no jitter)
            float multiplier = (index < frameTimeMultipliers.Length) 
                ? frameTimeMultipliers[index] 
                : 1f;

            float waitTime = baseFrameTime * multiplier;
            yield return new WaitForSeconds(waitTime);

            index = (index + 1) % idleFrames.Length;

            // Count full cycles (back to frame 0)
            if (index == 0)
            {
                cycleCount++;

                if (cycleCount >= nextPauseCycle)
                {
                    float pauseTime = Random.Range(pauseDurationMin, pauseDurationMax);
                    yield return new WaitForSeconds(pauseTime);

                    cycleCount = 0;
                    ResetPauseCycle();
                }
            }
        }
    }

    void ResetPauseCycle()
    {
        nextPauseCycle = Random.Range(minCyclesBeforePause, maxCyclesBeforePause + 1);
    }

    // Call this when swapping character
    public void SetIdleFrames(Sprite[] newFrames)
    {
        idleFrames = newFrames;
        index = 0;
        cycleCount = 0;
        ResetPauseCycle();
    }
}
