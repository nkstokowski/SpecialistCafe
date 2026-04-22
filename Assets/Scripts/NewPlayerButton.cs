using UnityEngine;
using UnityEngine.UI;

public class NewPlayerButton : MonoBehaviour
{

    public GameObject tutorialCanvas;
    public GameObject cafeTutorialBackground;

    public GameObject menuCanvas;
    public GameObject shopCanvas;

    public SwipeDetection swipeDetection;

    public void OnNewPlayerButtonSelected(Button button)
    {
        tutorialCanvas.SetActive(true);
        cafeTutorialBackground.SetActive(true);
        
        menuCanvas.SetActive(false);

        swipeDetection.disabled = true;
    }

    public void OnCloseTutorialselected(Button button)
    {
        tutorialCanvas.SetActive(false);
        cafeTutorialBackground.SetActive(false);

        menuCanvas.SetActive(true);

        swipeDetection.disabled = false;
    }
}
