using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenuUI : UI, ICardSelector
{
    private int editing = -1; // -1 for none

    [SerializeField] private List<Image> editSaveButtons;
    [SerializeField] private List<Image> selectButtons;
    [SerializeField] private List<TextMeshProUGUI> infoTexts;
    [SerializeField] private List<Image> upgradeButtons;
    [SerializeField] private TextMeshProUGUI upgradePointsDisplay;

    [SerializeField] private Vector2 positionOffset;
    private List<Character> characters;
    [SerializeField] private List<Transform> deckTransforms;
    private List<List<GameObject>> deckCardDisplays;
    [SerializeField] private Transform inventoryTransform;
    private List<GameObject> inventoryCardDisplay;

    [SerializeField] private GameObject cardPrefab;
    private List<ICardSelector> thisAsList;

    private int invOffset;
    private Vector3 initialInvOffset;

    void Awake()
    {
        thisAsList = new List<ICardSelector> { this };
        deckCardDisplays = new();
        for (int i = 0; i < deckTransforms.Count; i++)
        {
            deckCardDisplays.Add(new());
        }
        inventoryCardDisplay = new();

        initialInvOffset = new Vector3(inventoryTransform.localPosition.x, inventoryTransform.localPosition.y, 0);
    }

    public override void SetActive(bool b)
    {
        base.SetActive(b);
        if (b)
        {
            base.SetActive(b);
            GameManager.Instance.LoadUserData();
            GameManager.Instance.UpdateCardDeckLevels();
            characters = GameManager.Instance.usrData.characters;

            UpdateUpgradeInfos();

            DisplayAllCharacters();
            DisplayInventory();

            foreach (Image i in selectButtons) i.color = Color.gray;
            selectButtons[GameManager.Instance.usrData.selectedCharacter].color = Color.green;
        }
    }

    public override void Unload()
    {
        EditSaveCharacter(-1);

        base.Unload();
    }

    #region interactions

    public void SelectCard(int p)
    {
        if (editing == -1) return;
        else
        {
            if (p >= 8)
            {
                if (characters[editing].deck.Count > 7) return;
                p -= 8;
                characters[editing].deck.Add(GameManager.Instance.usrData.cards[p]);
                characters[editing].deck[characters[editing].deck.Count - 1].count = 1;
                DisplayDeck(characters[editing].deck, editing);
            }
            else
            {
                characters[editing].deck.RemoveAt(p);
                DisplayDeck(characters[editing].deck, editing);
            }
        }
    }

    public void SelectCharacter(int s) {
        selectButtons[GameManager.Instance.usrData.selectedCharacter].color = Color.gray;
        selectButtons[s].color = Color.green;
        GameManager.Instance.usrData.selectedCharacter = s;

        GameManager.Instance.SaveUserData();
    }

    public void ScrollRight()
    {
        Scroll(1);
    }

    public void ScrollLeft()
    {
        Scroll(-1);
    }

    private void Scroll(int d)
    {
        int max = inventoryCardDisplay.Count - 3;
        invOffset += d;
        if (invOffset < 0) invOffset = 0;
        if (invOffset > max) invOffset = max;
        inventoryTransform.localPosition = initialInvOffset + new Vector3(-invOffset * positionOffset.x, 0, 0);
    }

    public void EditSaveCharacter(int e)
    {
        if (editing != -1)
        {
            editSaveButtons[editing].color = Color.yellow;
            editSaveButtons[editing].GetComponentInChildren<TextMeshProUGUI>().text = "Edit";
        }

        if (editing == e) editing = -1;
        else
        {
            editing = e;
            if (e != -1)
            {
                editSaveButtons[e].color = Color.green;
                editSaveButtons[e].GetComponentInChildren<TextMeshProUGUI>().text = "Done!";
            }
        }

        if (editing == -1) {
        GameManager.Instance.SaveUserData();
        }
    }

    public void UpgradeCharacter(int i) {
        if (GameManager.Instance.usrData.upgradePoints < characters[i].GetRequiredUpgradePoints()) return;

        GameManager.Instance.usrData.upgradePoints -= characters[i].GetRequiredUpgradePoints();
        characters[i].SetLevel(characters[i].lvl + 1);
        
        GameManager.Instance.SaveUserData();

        UpdateUpgradeInfos();
    }

    #endregion

    #region display

    private void UpdateUpgradeInfos() {
        for (int i = 0; i < infoTexts.Count; i++)
        {
            infoTexts[i].text = $"{characters[i].type}\nLVL: {characters[i].lvl}\nHP: {characters[i].hp}";

            upgradeButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = $"Upgrade for\n{characters[i].GetRequiredUpgradePoints()} pts?";
            upgradeButtons[i].color = GameManager.Instance.usrData.upgradePoints >= characters[i].GetRequiredUpgradePoints() ? Color.green : Color.grey;

            upgradePointsDisplay.text = $"Upgradepoints:\n{GameManager.Instance.usrData.upgradePoints}";
        }
    }

    private void DisplayInventory()
    {
        List<Card> cards = GameManager.Instance.usrData.cards;

        foreach (GameObject obj in inventoryCardDisplay) obj.Destroy();
        inventoryCardDisplay = new List<GameObject>();

        int info = 0;

        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i].type > 5) {
                Vector3 pos = new Vector3(info++ * positionOffset.x, 0, 0);

                GameObject card = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity, inventoryTransform);
                card.transform.localPosition = pos;
                card.GetComponent<CardDisplay>().InitiateCardDisplay(cards[i]);
                card.GetComponent<CardDisplay>().InitiateSelectableCard(thisAsList, i + 8); // xD

                inventoryCardDisplay.Add(card);
            }
        }
    }

    private void DisplayDeck(List<Card> cards, int d)
    {
        foreach (GameObject obj in deckCardDisplays[d]) obj.Destroy();
        deckCardDisplays[d] = new List<GameObject>();

        for (int i = 0; i < cards.Count; i++)
        {
            int xOffset = i % 4;
            int yOffset = i / 4;
            Vector3 pos = new Vector3(xOffset * positionOffset.x, -yOffset * positionOffset.y, 0);

            GameObject card = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity, deckTransforms[d]);
            card.transform.localPosition = pos;
            card.GetComponent<CardDisplay>().InitiateCardDisplay(cards[i], hideCount:true);
            if (i >= 4) card.GetComponent<CardDisplay>().InitiateSelectableCard(thisAsList, i);

            deckCardDisplays[d].Add(card);
        }
    }

    private void DisplayAllCharacters()
    {
        for (int i = 0; i < deckTransforms.Count; i++)
        {
            DisplayDeck(characters[i].deck, i);
        }
    }
    
    #endregion
}
