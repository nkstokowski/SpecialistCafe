using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PageSwipe_Canvas : MonoBehaviour, IDragHandler, IEndDragHandler
{

    private Vector3 panelLocation;
    public float percentThreshold = 0.2f;
    public float easing = 0.5f;

    public int screenIndex = 1;
    public List<GameObject> screens;

    public void OnDrag(PointerEventData eventData)
    {

        float difference = eventData.pressPosition.x - eventData.position.x;
        transform.position = panelLocation - new Vector3(difference, 0, 0);

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float percentage = (eventData.pressPosition.x - eventData.position.x) / Screen.width;
        if(Mathf.Abs(percentage) >= percentThreshold) {
            Vector3 newLocation = panelLocation;

            if(percentage > 0) {
                newLocation += new Vector3(-Screen.width, 0, 0);
                screenIndex = Math.Max(0, screenIndex - 1);
                
            }
            else if(percentage < 0) {
                newLocation += new Vector3(Screen.width, 0, 0);
                screenIndex = Math.Min(screens.Count, screenIndex + 1);
            }
            
            Debug.Log("Starting drag");
            StartCoroutine(SmoothMove(transform.position, newLocation, easing));
            panelLocation = newLocation;
            Debug.Log("Done drag");
            // TODO - enable the canvas we are going to

        }
        else {
            StartCoroutine(SmoothMove(transform.position, panelLocation, easing));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        panelLocation = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SmoothMove(Vector3 startpos, Vector3 endpos, float seconds) {
        float t = 0f;
        while (t <= 1.0) {
            t += Time.deltaTime / seconds;
            transform.position = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }
}
