using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Enumeration;
using UnityEngine;
[CreateAssetMenu(fileName = "New Achievement", menuName = "Achievement")]
public class Achievement : ScriptableObject
{
    public int achievementID;
    public List<String> requirements;
    public Sprite leftImage;
    public Sprite rightImage;
    public String title;
    public String description;

    public bool VerifyRequirements(String firstGuest, String secondGuest)
    {
        return requirements.Contains(firstGuest) && requirements.Contains(secondGuest);
    }

}
