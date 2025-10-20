using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IAManager : MonoBehaviour, IDataPersistence
{

    // Achievements
    public List<int> unlockedAchievements;
    public List<Achievement> achievementList;
    public AchievementDisplay[] achievementDisplays;

    // Info
    public List<String> unlockedMenuItems;
    public List<InfoDisplay> allInfoBoxes;
    Dictionary<String, InfoDisplay> infoDisplayDict;

    void Start()
    {
        infoDisplayDict = new Dictionary<String, InfoDisplay>();
        foreach(InfoDisplay infDis in allInfoBoxes)
        {
            infoDisplayDict.Add(infDis.unlockMenuItem, infDis);
        }
    }

    public void LoadData(GameData data)
    {
        this.unlockedAchievements = data.unlockedAchievements;
        this.unlockedMenuItems = data.unlockedMenuItems;

        //UpdateAchievementList();
        UpdateUnlockedAchievements();
        UpdateUnlockedInfoBoxes();
    }

    public void SaveData(ref GameData data)
    {
        data.unlockedAchievements = this.unlockedAchievements;
    }

    /*void UpdateAchievementList()
    {
        foreach (Achievement ach in achievementList) {
            ach.unlocked = unlockedAchievements.Contains(ach.achievementID);
        }
    }*/

    public void UpdateUnlockedAchievements()
    {
        foreach (int index in this.unlockedAchievements)
        {
            if (index >= achievementDisplays.Length)
            {
                Debug.Log("Error: Tried to unlock achievement at index " + index + ". Only " + achievementDisplays.Length + " are known.");
            }
            else
            {
                Debug.Log("Setting achievement " + index + " to unlocked.");
                achievementDisplays[index].SetStatus(true);
            }
        }
    }

    public bool UnlockAchievement(int index)
    {
        if (this.unlockedAchievements.Contains(index))
        {
            Debug.Log("Tried to unlocked achievement: " + index + ". Already in list");
            return false;
        }

        if (index >= achievementDisplays.Length)
        {
            Debug.Log("Error: Tried to unlock achievement at index " + index + ". Only " + achievementDisplays.Length + " are known.");
            return false;
        }

        this.unlockedAchievements.Add(index);
        achievementDisplays[index].SetStatus(true);
        return true;
    }

    public void UpdateUnlockedInfoBoxes()
    {
        foreach (String menuItem in this.unlockedMenuItems)
        {
            if (!infoDisplayDict.ContainsKey(menuItem))
            {
                Debug.Log("Error: Menu Item " + menuItem + " not found in info boxes array.");
            }
            else
            {
                infoDisplayDict[menuItem].SetStatus(true);
            }
        }
    }
    
    public bool UnlockInfo(String menuItem)
    {
        if (!infoDisplayDict.ContainsKey(menuItem))
        {
            Debug.Log("Error: Menu Item " + menuItem + " not found in info boxes array.");
            return false;
        }

        infoDisplayDict[menuItem].SetStatus(true);
        return true;
    }
}
