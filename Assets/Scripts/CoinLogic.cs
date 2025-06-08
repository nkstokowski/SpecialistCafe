using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CoinLogic : MonoBehaviour, IPointerClickHandler
{

    SoundManager soundManager;
    MoneyManager moneyManager;

    CafeManager cafeManager;
    public int value;

    public bool lowerCoin;

    void Start()
    {
        moneyManager = GameObject.Find("MoneyManager").GetComponent<MoneyManager>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        cafeManager = GameObject.Find("CafeManager").GetComponent<CafeManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        moneyManager.AddMoney(value);
        soundManager.PlaySound("coin");
        cafeManager.CoinCollected(lowerCoin);
        Destroy(gameObject);
    }

}
