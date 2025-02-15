using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public List<GameObject> screenCanvasObjects;

    public void TransitionScreenUI(int previousScreen, int newScreen) {
        screenCanvasObjects[previousScreen].SetActive(false);
        screenCanvasObjects[newScreen].SetActive(true);
    }
}
