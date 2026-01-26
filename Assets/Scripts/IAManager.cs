using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IAManager : MonoBehaviour, IDataPersistence
{

    bool loaded = false;


    // Achievements
    public List<int> unlockedAchievements;
    public List<Achievement> achievementList;
    public GameObject achDisplayContainer;
    Dictionary<int, AchievementDisplay> achievementDisplayDict;
    public ExpManager expManager;
    public const int EXP_PER_ACHIEVEMENT = 10;
    String firstGuest;
    String secondGuest;

    // Info
    public List<String> unlockedMenuItems;
    public List<InfoDisplay> allInfoBoxes;
    Dictionary<String, InfoDisplay> infoDisplayDict;

    void Start()
    {
        infoDisplayDict = new Dictionary<String, InfoDisplay>();
        foreach (InfoDisplay infDis in allInfoBoxes)
        {
            infoDisplayDict.Add(infDis.unlockMenuItem, infDis);
        }

        achievementDisplayDict = new Dictionary<int, AchievementDisplay>();
        foreach(AchievementDisplay achDis in achDisplayContainer.GetComponentsInChildren<AchievementDisplay>())
        {
            achievementDisplayDict.Add(achDis.achievementID, achDis);
            //Debug.Log("Found Achievement with ID: " + achDis.achievementID);
        }
    }

    public void LoadData(GameData data)
    {
        //this.unlockedAchievements = data.unlockedAchievements;
        foreach(int ach in data.unlockedAchievements)
        {
            if (!this.unlockedAchievements.Contains(ach))
            {
                this.unlockedAchievements.Add(ach);
            }
        }

        this.unlockedMenuItems = data.unlockedMenuItems;

        //UpdateAchievementList();
        UpdateUnlockedAchievements();
        UpdateUnlockedInfoBoxes();
        if (firstGuest != null)
        {
            CheckGuests();
        }
        loaded = true;
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

            if (!achievementDisplayDict.ContainsKey(index))
            {
                Debug.Log("Error: Achivement with ID " + index + " not found in achivement dictionary");
            }
            else
            {
                achievementDisplayDict[index].SetStatus(true);
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

        if (!achievementDisplayDict.ContainsKey(index))
        {
            Debug.Log("Error: Achievement Id: " + index + " not recognized.");
            return false;
        }

        this.unlockedAchievements.Add(index);
        achievementDisplayDict[index].SetStatus(true);
        Debug.Log("Unlocked Achievement: " + index);
        expManager.ModifyExp(EXP_PER_ACHIEVEMENT);
        return true;
    }

    public void SetGuests(String guestA, String guestB)
    {
        this.firstGuest = guestA;
        this.secondGuest = guestB;

        if(loaded)
        {
            CheckGuests();
        }
    }

    bool CheckGuests()
    {
        foreach (Achievement achievement in this.achievementList)
        {
            if (!this.unlockedAchievements.Contains(achievement.achievementID))
            {
                if (achievement.VerifyRequirements(firstGuest, secondGuest))
                {
                    UnlockAchievement(achievement.achievementID);
                    return true;
                }
            }
        }
        return false;
    }

    public void UpdateUnlockedInfoBoxes()
    {
        foreach (String menuItem in this.unlockedMenuItems)
        {
            if (!infoDisplayDict.ContainsKey(menuItem))
            {
                Debug.Log("Error: Menu Item " + menuItem + " not found in info boxes dictionary.");
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
            Debug.Log("Error: Menu Item " + menuItem + " not found in info boxes dictionary.");
            return false;
        }

        infoDisplayDict[menuItem].SetStatus(true);
        return true;
    }
}
