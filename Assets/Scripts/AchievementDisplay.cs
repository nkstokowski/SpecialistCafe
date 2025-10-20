using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementDisplay : MonoBehaviour
{
    public Image[] displayElements;
    public TMP_Text[] textElements;
    public bool unlocked;
    public int LOCKED_ALPHA_VALUE = 50;
    public int UNLOCKED_ALPHA_VALUE = 255; 

    public void SetStatus(bool unlocked)
    {
        this.unlocked = unlocked;
        Color color;

        // Set alpha on images
        foreach (Image img in displayElements)
        {
            color = img.color;
            color.a = unlocked ? UNLOCKED_ALPHA_VALUE : LOCKED_ALPHA_VALUE;
            img.color = color;
        }
        
        // Set alpha on text
        foreach (TMP_Text txt in textElements)
        {
            color = txt.color;
            color.a = unlocked ? UNLOCKED_ALPHA_VALUE : LOCKED_ALPHA_VALUE;
            txt.color = color;
        }
    }
}
