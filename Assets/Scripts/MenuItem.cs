using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using UnityEngine;
[CreateAssetMenu(fileName = "New Menu Item", menuName = "Menu Item")]
public class MenuItem : ScriptableObject
{
    public String itemName;
    public String guestName;
    public Sprite guestSprite;
    public Sprite tableSprite;
    public Sprite chalkText;
    public float rarity;

    public float hourlyRate;
    public String theme;

    public GameObject coinsPrefab;

}
