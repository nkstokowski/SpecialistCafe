using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CoinLogic : MonoBehaviour, IPointerClickHandler
{

    public SoundManager soundManager;
    public MoneyManager moneyManager;

    public CafeManager cafeManager;
    public int value;

    public bool lowerCoin;

    public void OnPointerClick(PointerEventData eventData)
    {
        moneyManager.AddMoney(value);
        soundManager.PlaySound("coin");
        cafeManager.CoinCollected(lowerCoin);
        Destroy(gameObject);
    }

}
