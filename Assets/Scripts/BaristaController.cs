using System;
using System.Collections;
using UnityEngine;

public class BaristaController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private float moveDuration = 1.5f;

    [Header("Pause")]
    [SerializeField] private float interactSeconds = 1f;

    [Header("Random Wait Time")]
    [SerializeField] private float minWaitTime = 2f;
    [SerializeField] private float maxWaitTime = 5f;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string movingBoolName = "isWalking";
    [SerializeField] private string interactBoolName = "isInteracting";

    [Header("Sprite")]
    [SerializeField] private SpriteRenderer baristaSpriteRenderer;
    [SerializeField] private SpriteRenderer hatSpriteRenderer;

    private float waitTimer;
    private float currentWaitTime;
    private bool isBusy;

    private void Start()
    {
        RandomizeWaitTime();
    }

    private void Update()
    {
        if (isBusy)
            return;

        waitTimer += Time.deltaTime;

        if (waitTimer >= currentWaitTime)
        {
            waitTimer = 0f;
            StartCoroutine(MoveForwardThenBackward());
        }
    }

    private IEnumerator MoveForwardThenBackward()
    {
        isBusy = true;

        // ---- Move Forward ----
        animator.SetBool(movingBoolName, true);
        yield return StartCoroutine(MoveCoroutine(
            startPoint.position,
            endPoint.position
        ));
        animator.SetBool(movingBoolName, false);

        // ---- Pause ----
        animator.SetBool(interactBoolName, true);
        yield return new WaitForSeconds(interactSeconds);
        animator.SetBool(interactBoolName, false);

        // ---- Move Backward ----
        baristaSpriteRenderer.flipX = true;
        hatSpriteRenderer.flipX = true;
        animator.SetBool(movingBoolName, true);
        yield return StartCoroutine(MoveCoroutine(
            endPoint.position,
            startPoint.position
        ));
        animator.SetBool(movingBoolName, false);
        baristaSpriteRenderer.flipX = false;
        hatSpriteRenderer.flipX = false;

        RandomizeWaitTime();
        isBusy = false;
    }

    private IEnumerator MoveCoroutine(Vector3 from, Vector3 to)
    {
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / moveDuration;
            transform.position = Vector3.Lerp(from, to, t);
            yield return null;
        }

        transform.position = to;
    }

    private void RandomizeWaitTime()
    {
        currentWaitTime = UnityEngine.Random.Range(minWaitTime, maxWaitTime);
    }
}