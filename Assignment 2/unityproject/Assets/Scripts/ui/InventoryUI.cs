using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : UI
{
    [SerializeField] private Transform cardDisplayTransform;
    [SerializeField] private int cardsPerRow;
    [SerializeField] private Vector2 positionOffset;
    [SerializeField] private GameObject cardPrefab;
    [Space]
    [SerializeField] private TextMeshProUGUI labelGold;
    [SerializeField] private TextMeshProUGUI labelUpgradePoints;
    private List<Card> cards;
    private List<GameObject> cardDisplay = new List<GameObject>();

    public override void SetActive(bool b)
    {
        base.SetActive(b);

        if (b)
        {
            GameManager.Instance.LoadUserData();
            User usr = GameManager.Instance.usrData;

            labelGold.text = "Gold: " + usr.gold;
            labelUpgradePoints.text = "Upgrade Points: " + usr.upgradePoints;
            
            DisplayInventory();
        }
    }

    public override void Unload()
    {
        ClearInventoryDisplay();

        base.Unload();
    }

    public void DisplayInventory()
    {
        List<Card> cards = GameManager.Instance.usrData.cards;

        cardDisplay = new List<GameObject>();
        for (int i = 0; i < cards.Count; i++)
        {
            int xOffset = i % cardsPerRow;
            int yOffset = i / cardsPerRow;
            Vector3 pos = new Vector3(xOffset * positionOffset.x, -yOffset * positionOffset.y, 0);

            GameObject card = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity, cardDisplayTransform);
            card.transform.localPosition = pos;
            card.GetComponent<CardDisplay>().InitiateCardDisplay(cards[i]);

            cardDisplay.Add(card);
        }
    }

    public void ClearInventoryDisplay()
    {
        foreach (GameObject obj in cardDisplay)
        {
            Destroy(obj);
        }
    }
}
