using System.Collections;
using System.Collections.Generic;
using Mapbox.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleArenaUI : MiniGameUI, ICardSelector
{
    #region general

    private BattleArena data;

    [SerializeField] private GameObject enemyDisplay;
    [SerializeField] private Transform playerCardDisplayCenter;
    [SerializeField] private Transform enemyCardDisplayCenter;
    [SerializeField] private Transform teamCardDisplayCenter;
    [SerializeField] private GameObject teamStatsDisplay;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject lossScreen;
    [SerializeField] private GameObject rewardScreen;

    #endregion

    [SerializeField] private Slider bossHpBar;
    [SerializeField] private Slider bossDmgBar;
    [SerializeField] private TextMeshProUGUI goldDisplay;
    [SerializeField] private TextMeshProUGUI upgradeDisplay;
    [SerializeField] private Transform rewardCardDisplayCenter;

    #region cardDisplays

    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Vector2 positionOffset;
    [SerializeField] private float teamCardDisplayScaleFactor;

    private List<GameObject> playerCardDisplay = new List<GameObject>();
    private List<GameObject> enemyCardDisplay = new List<GameObject>();
    private List<GameObject> teamCardDisplay = new List<GameObject>();

    private List<ICardSelector> cardSelectors;

    #endregion

    [SerializeField] private TextMeshProUGUI healthLabel;
    [SerializeField] private TextMeshProUGUI shieldLabel;


    public override void InitiateMiniGame(DungeonUI ui, Dungeon d)
    {
        base.InitiateMiniGame(ui, d);

        cardSelectors = new List<ICardSelector> { this, BattleManager.Instance };

        data = JsonConvert.DeserializeObject<BattleArena>(d.miniGameJson);

        enemyDisplay.GetComponent<Image>().sprite = GameAssetManager.Instance.ReadEnemy(data.enemy.enemyType).sprite;

        bossHpBar.maxValue = data.enemy.hp;
        bossDmgBar.maxValue = data.enemy.hp;

        SwitchState(BattleState.Initial);
    }

    public void SelectCard(int p)
    {
        Debug.Log("Selected Card: " + p);
    }

    #region control methods

    private void SwitchState(BattleState s)
    {
        BattleManager.Instance.battleState = s;
        switch (s)
        {
            case BattleState.Initial:
                {
                    enemyDisplay.SetActive(true);
                    playerCardDisplayCenter.gameObject.SetActive(true);
                    enemyCardDisplayCenter.gameObject.SetActive(true);
                    teamCardDisplayCenter.gameObject.SetActive(false);
                    teamStatsDisplay.SetActive(true);
                    winScreen.SetActive(false);
                    lossScreen.SetActive(false);
                    rewardScreen.SetActive(false);
                    break;
                }
            case BattleState.SelectCard:
                {
                    teamCardDisplayCenter.gameObject.SetActive(true);

                    List<Card> enemyCards = new List<Card>();
                    for (int i = 0; i < GameManager.Instance.usrParty.memberCount - 1; i++) enemyCards.Add(new Card(0));
                    DisplayEnemyCards(enemyCards);
                    DisplayTeamCards(new List<Card>());
                    break;
                }
            case BattleState.EvaluateRound:
                {
                    teamCardDisplayCenter.gameObject.SetActive(false);
                    break;
                }
            case BattleState.Rewards:
                {
                    enemyDisplay.SetActive(false);
                    playerCardDisplayCenter.gameObject.SetActive(false);
                    enemyCardDisplayCenter.gameObject.SetActive(false);
                    teamCardDisplayCenter.gameObject.SetActive(false);
                    teamStatsDisplay.SetActive(false);
                    winScreen.SetActive(false);
                    lossScreen.SetActive(false);
                    rewardScreen.SetActive(true);
                    break;
                }
            default: break;
        }
    }

    // call this to start each round
    public void SwitchToCardSelector(List<Card> draw, int teamHP, int teamShield, int enemyHP)
    {
        SwitchState(BattleState.SelectCard);

        DisplayPlayerCards(draw, true);
        UpdateStats(teamHP, teamShield);

        bossHpBar.value = enemyHP;
        bossDmgBar.value = enemyHP;
    }

    //call this to begin evaluation after all members have selected their card for the round
    public void SwitchToEvaluation(List<Card> teamCards, List<Card> enemyCards, int teamHP, int teamShield, int enemyHP)
    {
        SwitchState(BattleState.EvaluateRound);

        DisplayPlayerCards(teamCards, false);
        DisplayEnemyCards(enemyCards);
        UpdateStats(teamHP, teamShield);

        bossHpBar.value = enemyHP;
    }

    public void SwitchToWinScreen() {
        winScreen.SetActive(true);
    }

    public void SwitchToLossScreen() {
        lossScreen.SetActive(true);
    }

    public void SwitchToRewards() {
        SwitchState(BattleState.Rewards);
        BattleManager.Instance.CollectRewards();

        goldDisplay.text = "+" + data.rewardGold + " Gold";
        upgradeDisplay.text = "+" + data.rewardUpgradePoints + " Upgrade Points";

        List<Card> cards = data.rewardCards;
        for (int i = 0; i < cards.Count; i++)
        {
            float xOffset = i * positionOffset.x - ((cards.Count - 1) * positionOffset.x / 2);
            Vector3 pos = new Vector3(xOffset, 0);

            GameObject card = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity, rewardCardDisplayCenter);
            card.transform.localPosition = pos;
            CardDisplay c = card.GetComponent<CardDisplay>();
            c.InitiateCardDisplay(cards[i], hideLVL: true);
        }
    }

    public void PressSwitchToRewards() {
        SwitchToRewards();
    }

    // call this each time a card was selected by the team
    public void DisplayTeamCards(List<Card> draw) {
        foreach (GameObject g in teamCardDisplay)
        {
            Destroy(g);
        }
        teamCardDisplay = new List<GameObject>();
        for (int i = 0; i < draw.Count; i++)
        {
            float yOffset = -i * positionOffset.y * teamCardDisplayScaleFactor;
            Vector3 pos = new Vector3(0, yOffset);

            GameObject card = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity, teamCardDisplayCenter);
            card.transform.localPosition = pos;
            card.transform.localScale = Vector3.one * teamCardDisplayScaleFactor;
            CardDisplay c = card.GetComponent<CardDisplay>();
            c.InitiateCardDisplay(draw[i], hideCount:true);

            teamCardDisplay.Add(card);
        }
    }

    #endregion

    #region internal methods

    // dont call this on its own
    private void DisplayPlayerCards(List<Card> draw, bool interactable) {
        foreach (GameObject g in playerCardDisplay)
        {
            Destroy(g);
        }
        playerCardDisplay = new List<GameObject>();
        for (int i = 0; i < draw.Count; i++)
        {
            float xOffset = i * positionOffset.x - ((draw.Count - 1) * positionOffset.x / 2);
            Vector3 pos = new Vector3(xOffset, 0);

            GameObject card = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity, playerCardDisplayCenter);
            card.transform.localPosition = pos;
            CardDisplay c = card.GetComponent<CardDisplay>();
            c.InitiateCardDisplay(draw[i], hideCount:true);
            if (interactable) c.InitiateSelectableCard(cardSelectors, i);

            playerCardDisplay.Add(card);
        }
    }

    // dont call this on its own
    private void DisplayEnemyCards(List<Card> draw) {
        foreach (GameObject g in enemyCardDisplay)
        {
            Destroy(g);
        }
        enemyCardDisplay = new List<GameObject>();
        for (int i = 0; i < draw.Count; i++)
        {
            float xOffset = i * positionOffset.x - ((draw.Count - 1) * positionOffset.x / 2);
            Vector3 pos = new Vector3(xOffset, 0);

            GameObject card = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity, enemyCardDisplayCenter);
            card.transform.localPosition = pos;
            CardDisplay c = card.GetComponent<CardDisplay>();
            c.InitiateCardDisplay(draw[i], hideCount:true, displayAsEnemy: true);

            enemyCardDisplay.Add(card);
        }
    }


    private void UpdateStats(int teamHP, int teamShield) {
        healthLabel.text = "HP: " + teamHP;
        shieldLabel.text = "Shield: " + teamShield;
    }

    public void ExitArena() {
        GameManager.Instance.SetUIActive(GameManager.Instance.allUIs[1]); // ¯\_(ツ)_/¯
    }

    public void CollectRewards() {
        ExitArena();
    }

    #endregion
}

public enum BattleState
{
    Initial,
    SelectCard,
    EvaluateRound,
    Win,
    Lose,
    Rewards,
    Waiting
}