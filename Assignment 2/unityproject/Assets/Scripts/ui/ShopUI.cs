using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ShopUI : UI
{
    [SerializeField] GameObject cardsInfoPanel;

    [SerializeField] TextMeshProUGUI notificationLabel;

    [SerializeField] TextMeshProUGUI goldLabel;

    bool cardsInfoPanelActivated = false;

    /*int DAGGER_PRICE = 10;
    int GREATSWORD_PRICE = 20;
    int POTION_PRICE = 25;
    int KATANA_PRICE = 15;
    int SHIELD_PRICE = 10;
    int STRENGTH_PRICE = 20; */

    Dictionary<int, int> priceMap = new Dictionary<int, int>{
        { 1, 10 }, { 2, 20 }, { 3, 25 }, { 4, 15 }, { 5, 10 }, { 16, 20 }
    };

    int shopLVL = 4;
    
    public override void SetActive(bool b)
    {
        base.SetActive(b);

        if (b)
        {
            // This has to be changed to access the GameManagers user data instead
            User usr = GameManager.Instance.usrData;
            if (usr != null)
            {
                goldLabel.text = "Your Gold: " + usr.gold;
            }
            notificationLabel.text = "";
        }
    }


    // ================================ Button functionality ===========================
    public void CardsInfoButtonClicked()
    {
        if (cardsInfoPanelActivated)
        {
            return;
        }
        cardsInfoPanelActivated = true;
        cardsInfoPanel.SetActive(true);
    }

    public void CloseButtonClicked()
    {
        cardsInfoPanelActivated = false;
        cardsInfoPanel.SetActive(false);
    }

    public void LeaveButtonClicked()
    {
        if (cardsInfoPanelActivated)
        {
            return;
        }
        MapUI mapUI = GameObject.Find("UI").GetComponentInChildren<MapUI>(true);
        if (mapUI != null)
        {
            LoadUI(mapUI);
        }
    }

    // ==================== Buy Buttons =================================

    private void BuyItem(int id)
    {
        User userData = GameManager.Instance.usrData;
        if (userData.gold >= priceMap[id])
        {
            userData.gold -= priceMap[id];

            Card cardToBeAdded = new Card(id, shopLVL, 1);

            GameManager.Instance.AddCardsToInventory(new List<Card> { cardToBeAdded });

            GameManager.Instance.SaveUserData();

            goldLabel.text = "Your Gold: " + GameManager.Instance.usrData.gold.ToString();
            notificationLabel.text = "Bought an item!";
        }
        else
        {
            notificationLabel.text = "Not enough gold!";
        }
    }

    public void BuyDaggerButtonClicked()
    {
        BuyItem(1);
    }

    public void BuyGreatswordButtonClicked()
    {
        BuyItem(2);
    }

    public void BuyPotionButtonClicked()
    {
        BuyItem(3);
    }

    public void BuyKatanaButtonClicked()
    {
        BuyItem(4);
    }

    public void BuyShieldButtonClicked()
    {
        BuyItem(5);
    }

    public void BuyStrengthPotionButtonClicked()
    {
        BuyItem(16);
    }

}
