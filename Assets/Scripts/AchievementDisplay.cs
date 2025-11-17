using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AchievementDisplay : MonoBehaviour
{

    public Achievement achievement;
    public Image[] displayElements;
    public TMP_Text[] textElements;
    public bool unlocked;
    public int LOCKED_ALPHA_VALUE = 50;
    public int UNLOCKED_ALPHA_VALUE = 255;
    public int achievementID;

    void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        //this.displayElements[1].sprite = this.achievement.leftImage;
        //this.displayElements[2].sprite = this.achievement.rightImage;

        //this.textElements[0].text = this.achievement.title;

        this.textElements[0].text = this.achievement.description == "" ?
            "Have a " + this.achievement.requirements[0] + " and a " + this.achievement.requirements[1] + " visit your cafe at the same time" :
            this.achievement.description;
    }

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
