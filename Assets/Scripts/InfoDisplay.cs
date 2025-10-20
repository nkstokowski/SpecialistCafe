using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoDisplay : MonoBehaviour
{
    public GameObject[] unlockedElements;
    public GameObject[] hiddenElements;
    public bool unlocked;
    public string unlockMenuItem;

    public void SetStatus(bool unlocked)
    {
        this.unlocked = unlocked;

        // Show/Hide the unlocked elements 
        foreach (GameObject obj in unlockedElements)
        {
            obj.SetActive(unlocked);
        }

        // Show/Hide the locked elements
        foreach (GameObject obj in hiddenElements)
        {
            obj.SetActive(!unlocked);
        }
    }
}
