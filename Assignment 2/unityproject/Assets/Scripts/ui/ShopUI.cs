using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : UI
{
    [SerializeField] GameObject cardsInfoPanel;

    [SerializeField] TextMeshProUGUI notificationLabel;

    [SerializeField] TextMeshProUGUI goldLabel;

    bool cardsInfoPanelActivated = false;
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





}
