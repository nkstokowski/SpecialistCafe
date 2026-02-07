using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using UnityEngine;
[CreateAssetMenu(fileName = "New Hat", menuName = "Hat")]
public class Hat : ScriptableObject
{
    public int id;
    public string themeName;
    public bool unlocked;

    public Sprite hatSprite;
    public Sprite shopSprite;

}
