using System;
using System.Collections;
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
    private UIManager uiManager;

    private Vector2 startPosition;
    private float startTime;
    
    private Vector2 endPosition;
    private float endTime;

    private Coroutine trailCoroutine;
    
    public int currentScreen;

    private void Awake() {
        inputManager = InputManager.Instance;
        uiManager = GetComponent<UIManager>();
        currentScreen = 1;
    }

    private void OnEnable() {
        inputManager.OnStartTouch += SwipeStart;
        inputManager.OnEndTouch += SwipeEnd;
    }

    private void OnDisable() {
        inputManager.OnStartTouch -= SwipeStart;
        inputManager.OnEndTouch -= SwipeEnd;
    }


    private void SwipeStart(Vector2 position, float time) {
        startPosition = position;
        startTime = time;
        trail.SetActive(true);
        trail.transform.position = position;
        trailCoroutine = StartCoroutine(Trail());
    }

    private void SwipeEnd(Vector2 position, float time) {
        trail.SetActive(false);
        StopCoroutine(trailCoroutine);
        endPosition = position;
        endTime = time;
        DetectSwipe();
    }

    private IEnumerator Trail() {
        while(true) {
            trail.transform.position = inputManager.PrimaryPosition();
            yield return null;
        }
    }

    private void DetectSwipe() {
        if((Vector2.Distance(startPosition, endPosition) >= minimumDistance) &&
            ((endTime - startTime) <= maximumTime)) {
                Debug.DrawLine(startPosition, endPosition, Color.red, 5f);
                Vector2 direction = endPosition - startPosition;
                Vector2 direction2D = new Vector2(direction.x, direction.y).normalized;
                SwipeDirection(direction2D);
        }
    }

    private void SwipeDirection(Vector2 direction) {
        
        int newScreen = currentScreen;

        if (Vector2.Dot(Vector2.up, direction) > directionThreshold) {
            Debug.Log("Swipe Up");
        }
        if (Vector2.Dot(Vector2.down, direction) > directionThreshold) {
            Debug.Log("Swipe Down");
        }
        if (Vector2.Dot(Vector2.left, direction) > directionThreshold) {
            Debug.Log("Swipe Left");
            newScreen = Math.Min(2, currentScreen + 1);
            OnSwipeLeft();
        }
        if (Vector2.Dot(Vector2.right, direction) > directionThreshold) {
            Debug.Log("Swipe Right");
            newScreen = Math.Max(0, currentScreen - 1);
            OnSwipeRight();
        }

        //uiManager.TransitionScreenUI(currentScreen, newScreen);
        currentScreen = newScreen;
    }

    private void OnSwipeRight() {
        Vector3 newLocation = inputManager.mainCamera.transform.position;
        newLocation += new Vector3(-5.66f, 0, 0);
        StartCoroutine(SmoothMove(inputManager.mainCamera.transform.position, newLocation, .5f));
    }

    private void OnSwipeLeft() {
        Vector3 newLocation = inputManager.mainCamera.transform.position;
        newLocation += new Vector3(5.66f, 0, 0);
        StartCoroutine(SmoothMove(inputManager.mainCamera.transform.position, newLocation, .5f));
    }

    IEnumerator SmoothMove(Vector3 startpos, Vector3 endpos, float seconds) {
        float t = 0f;
        while (t <= 1.0) {
            t += Time.deltaTime / seconds;
            inputManager.mainCamera.transform.position = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }
}