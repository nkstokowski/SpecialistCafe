using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CafeStateManager : MonoBehaviour
{

    public List<MenuItem> menuList;
    public List<GameObject> allIngridients;
    Dictionary<String, GameObject> ingridientsDict;
    GameObject guestPrefab;

    Vector3 upperGuestPos = new Vector3(-1.65999997f,-0.349999994f,0f);
    Vector3 lowerGuestPos = new Vector3(1.87f,-2.38000011f,0f);

    Vector3 upperDrinkPos = new Vector3(-1.06900001f,-0.757000029f,0f);
    Vector3 lowerDrinkPos = new Vector3(1.31700003f,-2.773f,0f);

    // Start is called before the first frame update
    void Start()
    {
        InitializeCafeData();
        PopulateCafe();
    }

    void InitializeCafeData() {

        ingridientsDict = new Dictionary<String, GameObject>();

        // Turn the ingridients list into a dictionary
        foreach(GameObject ingridientObj in allIngridients) {
            ingridientsDict.Add(ingridientObj.name, ingridientObj);
        }
    }

    void PopulateCafe() {
        if(menuList.Count > 0) {
            //PopulateShelf();
            PopulateChalkboard();
            PopulateTables();
        }
    }

    private void PopulateTables()
    {
        int upperGuestIndex = UnityEngine.Random.Range(0, menuList.Count);
        if(menuList.Count > 1) {
            int lowerGuestIndex;

            do{
                lowerGuestIndex = UnityEngine.Random.Range(0, menuList.Count);
            } while (lowerGuestIndex == upperGuestIndex);

            Debug.Log("Lower Guest: " + menuList[lowerGuestIndex].guestName);
            spawnGuest(menuList[lowerGuestIndex], true);
        }
        Debug.Log("Upper Guest: " + menuList[upperGuestIndex].guestName);
        spawnGuest(menuList[upperGuestIndex], false);
    }

    private void spawnGuest(MenuItem menuItem, Boolean lowerGuest) {

        // Guest
        GameObject guest = new GameObject(menuItem.guestName, typeof(SpriteRenderer));
        guest.GetComponent<SpriteRenderer>().sprite = menuItem.guestSprite;
        guest.GetComponent<SpriteRenderer>().flipX = lowerGuest;
        guest.transform.position = lowerGuest ? lowerGuestPos : upperGuestPos;
        guest.transform.localScale = new Vector3(.5f, .5f, 1f);

        // Drink
        GameObject drink = new GameObject(menuItem.guestName + " Drink", typeof(SpriteRenderer));
        drink.GetComponent<SpriteRenderer>().sprite = menuItem.tableSprite;
        drink.GetComponent<SpriteRenderer>().flipX = lowerGuest;
        drink.transform.position = lowerGuest ? lowerDrinkPos : upperDrinkPos;
        drink.transform.localScale = new Vector3(.5f, .5f, 1f);
    }

    private void PopulateChalkboard()
    {
        //throw new NotImplementedException();
    }

    void PopulateShelf() {
        foreach(MenuItem item in menuList) {
            ingridientsDict[item.itemName].SetActive(true);
        }
    }
}
