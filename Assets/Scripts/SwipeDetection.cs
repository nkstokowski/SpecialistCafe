using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{

    [SerializeField]
    private float minimumDistance = 0.2f;
    [SerializeField]
    private float maximumTime = 1f;
    [SerializeField, Range(0f, 1f)]
    private float directionThreshold = .9f;
    [SerializeField]
    private GameObject trail;

    private InputManager inputManager;
    public UIManager uiManager;

    private Vector2 startPosition;
    private float startTime;

    private Vector2 endPosition;
    private float endTime;

    private Coroutine trailCoroutine;

    public UIScreen currentScreen = UIScreen.Center;

    public enum UIScreen { Left, Center, Right }
    Dictionary<UIScreen, Vector3> screenPositions = new Dictionary<UIScreen, Vector3>(){
        {UIScreen.Left, new Vector3(-5.66f,0f,-10f)},
        {UIScreen.Center, new Vector3(0f,0f,-10f)},
        {UIScreen.Right, new Vector3(5.66f,0f,-10f)}
    };

    bool moving = false;

    private void Awake()
    {
        inputManager = InputManager.Instance;
        uiManager = GetComponent<UIManager>();
        currentScreen = UIScreen.Center;
    }

    private void OnEnable()
    {
        inputManager.OnStartTouch += SwipeStart;
        inputManager.OnEndTouch += SwipeEnd;
    }

    private void OnDisable()
    {
        inputManager.OnStartTouch -= SwipeStart;
        inputManager.OnEndTouch -= SwipeEnd;
    }


    private void SwipeStart(Vector2 position, float time)
    {
        startPosition = position;
        startTime = time;
        trail.SetActive(true);
        trail.transform.position = position;
        trailCoroutine = StartCoroutine(Trail());
    }

    private void SwipeEnd(Vector2 position, float time)
    {
        trail.SetActive(false);
        StopCoroutine(trailCoroutine);
        endPosition = position;
        endTime = time;
        DetectSwipe();
    }

    private IEnumerator Trail()
    {
        while (true)
        {
            trail.transform.position = inputManager.PrimaryPosition();
            yield return null;
        }
    }

    private void DetectSwipe()
    {
        if ((Vector2.Distance(startPosition, endPosition) >= minimumDistance) &&
            ((endTime - startTime) <= maximumTime))
        {
            Debug.DrawLine(startPosition, endPosition, Color.red, 5f);
            Vector2 direction = endPosition - startPosition;
            Vector2 direction2D = new Vector2(direction.x, direction.y).normalized;
            SwipeDirection(direction2D);
        }
    }

    private void SwipeDirection(Vector2 direction)
    {

        if (Vector2.Dot(Vector2.up, direction) > directionThreshold)
        {
            Debug.Log("Swipe Up");
        }
        if (Vector2.Dot(Vector2.down, direction) > directionThreshold)
        {
            Debug.Log("Swipe Down");
        }
        if (Vector2.Dot(Vector2.left, direction) > directionThreshold)
        {
            Debug.Log("Swipe Left");
            OnSwipe(true);
        }
        if (Vector2.Dot(Vector2.right, direction) > directionThreshold)
        {
            Debug.Log("Swipe Right");
            OnSwipe(false);
        }
    }

    private void OnSwipe(bool forward)
    {
        UIScreen newScreen = nextScreen(forward);
        Vector3 newLocation = screenPositions[newScreen];
        if (!moving && newScreen != currentScreen)
        {
            if (newScreen != UIScreen.Right) uiManager.HideShop();
            currentScreen = newScreen;
            moving = true;
            StartCoroutine(SmoothMove(inputManager.mainCamera.transform.position, newLocation, .5f));
        }
    }

    IEnumerator SmoothMove(Vector3 startpos, Vector3 endpos, float seconds)
    {
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            inputManager.mainCamera.transform.position = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
        moving = false;
        if (currentScreen == UIScreen.Right) uiManager.ShowShop();
    }

    UIScreen nextScreen(bool forward)
    {
        switch (currentScreen)
        {
            case UIScreen.Left:
                return forward ? UIScreen.Center : currentScreen;
            case UIScreen.Center:
                return forward ? UIScreen.Right : UIScreen.Left;
            case UIScreen.Right:
                return forward ? currentScreen : UIScreen.Center;
        }

        return currentScreen;
    }
}