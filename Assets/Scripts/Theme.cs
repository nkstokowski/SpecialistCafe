using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using UnityEngine;
[CreateAssetMenu(fileName = "New Theme", menuName = "Theme")]
public class Theme : ScriptableObject
{
    public string themeName;
    public bool unlocked;

    public Sprite table;
    public Sprite chair;
    public Sprite wall;
    public Sprite counter;
    public Sprite floor;
    public Sprite icon;
}
