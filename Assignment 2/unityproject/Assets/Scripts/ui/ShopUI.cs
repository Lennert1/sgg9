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
        { 1, 10 }, { 2, 20 }, { 3, 25 }, { 4, 15 }, { 5, 10 }, { 6, 20 }
    };

    int shopLVL = 4;
    
    public override void SetActive(bool b)
    {
        base.SetActive(b);

        if (b)
        {
            // This has to be changed to access the GameManagers user data instead
            User userData = GameManager.Instance.GetAPI().LoadUserDataFromFile();
            if (userData != null)
            {
                goldLabel.text = "Your Gold: " + userData.gold;
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

            if (!CheckIfCardExists(cardToBeAdded))
            {
                userData.cards.Add(cardToBeAdded);
            }
            // Update the userData.json file, don't think we need this once the data base exists
            GameManager.Instance.UpdateUserData(userData);

            API api = GameManager.Instance.GetAPI();
            

            UpdateData updateData = new UpdateData
            {
                updatedGold = userData.gold,
                updatedCards = userData.cards
            };

            // Send update information to the server
            StartCoroutine(api.SendUpdate(updateData));

            goldLabel.text = "Your Gold: " + GameManager.Instance.usrData.gold.ToString();
            notificationLabel.text = "Bought an item!";
        }
        else
        {
            notificationLabel.text = "Not enough gold!";
        }
    }

    private bool CheckIfCardExists(Card cardToBeAdded)
    {
        foreach (Card card in GameManager.Instance.usrData.cards)
        {
            if (card.type == cardToBeAdded.type && card.lvl == cardToBeAdded.lvl)
            {
                card.count++;
                return true;
            }
        }
        return false;
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
        BuyItem(6);
    }

}
