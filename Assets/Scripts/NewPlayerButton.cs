using System;
using UnityEngine;
using UnityEngine.UI;

public class NewPlayerButton : MonoBehaviour
{

    public GameObject tutorialCanvas;
    public GameObject cafeTutorialBackground;
    public SwipeDetection swipeDetectionManager;

    public GameObject menuCanvas;
    public GameObject shopCanvas;

    public SwipeDetection swipeDetection;

    public void OnNewPlayerButtonSelected(Button button)
    {
        StartTutorial();
    }

    public void OnCloseTutorialselected(Button button)
    {
        EndTutorial();
    }

    public void StartTutorial()
    {
        tutorialCanvas.SetActive(true);
        cafeTutorialBackground.SetActive(true);
        
        SetEnabledCurrentScreen(false);

        swipeDetection.disabled = true;
    }

    private void SetEnabledCurrentScreen(Boolean state)
    {
        switch(swipeDetectionManager.currentScreen)
        {
            case SwipeDetection.UIScreen.Left:
                menuCanvas.SetActive(state);
                break;
            case SwipeDetection.UIScreen.Right:
                shopCanvas.SetActive(state);
                break;
        }
    }

    public void EndTutorial()
    {
        tutorialCanvas.SetActive(false);
        cafeTutorialBackground.SetActive(false);

        SetEnabledCurrentScreen(true);

        swipeDetection.disabled = false;
    }
}
