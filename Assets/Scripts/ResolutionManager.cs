using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    public CafeManager cafeManager;
    public SwipeDetection swipeDetection;
    public float baselineAspectHeight = 16f;
    public float baselineAspectWidth = 9f;

    public float aspectScaler;

    void Awake()
    {

        float cameraAspect = Camera.main.aspect;
        float baselineAspect = baselineAspectWidth / baselineAspectHeight;
        float aspectScaler = (cameraAspect / baselineAspect);

        //Debug.Log("Baseline Aspect: " + baselineAspect);
        //Debug.Log("Current Aspect: " + cameraAspect);
        //Debug.Log("Aspect Percentage: " + aspectScaler);

        cafeManager.SetScreensRatio(aspectScaler);
        swipeDetection.aspectScaler = aspectScaler;
    }
}
