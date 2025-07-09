using UnityEngine;


public class ResolutionManager : MonoBehaviour
{

    public GameObject screenPlaceholder;
    public float screenWorldWidth;
    public float screenWorldHeight;
    public float horizontalMin;
    public float horizontalMax;

    void Awake()
    {
        Camera camera = Camera.main;
        float halfHeight = camera.orthographicSize;
        float halfWidth = camera.aspect * halfHeight;

        screenWorldWidth = 2f * halfWidth;
        screenWorldHeight = 2f * halfHeight;

        horizontalMin = -halfWidth;
        horizontalMax = halfWidth;
    }

    void Update()
    {
        Camera camera = Camera.main;
        float halfHeight = camera.orthographicSize;
        float halfWidth = camera.aspect * halfHeight;

        float horizontalMin = -halfWidth;
        float horizontalMax = halfWidth;

        //Debug.Log("My width: " + (2f * halfWidth));
    }


}
