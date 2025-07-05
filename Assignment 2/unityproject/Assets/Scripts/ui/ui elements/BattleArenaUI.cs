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
    private BattleState state;

    [SerializeField] private GameObject enemyDisplay;
    [SerializeField] private Transform playerCardDisplayCenter;
    [SerializeField] private Transform enemyCardDisplayCenter;
    [SerializeField] private Transform teamCardDisplayCenter;
    [SerializeField] private GameObject teamStatsDisplay;

    #endregion

    [SerializeField] private Slider bossHpBar;
    [SerializeField] private Slider bossDmgBar;

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

#warning missing: display enemy sprite

        bossHpBar.maxValue = data.boss.hp;
        bossDmgBar.maxValue = data.boss.hp;

        SwitchState(BattleState.Initial);
    }

    public void SelectCard(int p)
    {
        Debug.Log("Selected Card: " + p);
    }

    #region control methods

    private void SwitchState(BattleState s)
    {
        state = s;

        switch (s)
        {
            case BattleState.Initial:
                {
                    enemyDisplay.SetActive(true);
                    playerCardDisplayCenter.gameObject.SetActive(true);
                    enemyCardDisplayCenter.gameObject.SetActive(true);
                    teamCardDisplayCenter.gameObject.SetActive(false);
                    teamStatsDisplay.SetActive(true);
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
            case BattleState.Win:
                {
                    break;
                }
            case BattleState.Lose:
                {
                    break;
                }
            case BattleState.Rewards:
                {
                    break;
                }
            default: Debug.LogError("How tf did you manage to have an incorrect battle state?"); break;
        }
    }

    // call this to start each round
    public void SwitchToCardSelector(List<Card> draw, int teamHP, int teamShield, int enemyHP)
    {
        SwitchState(BattleState.SelectCard);

        DisplayPlayerCards(draw);
        UpdateStats(teamHP, teamShield);

        bossHpBar.value = enemyHP;
        bossDmgBar.value = enemyHP;
    }

    //call this to begin evaluation after all members have selected their card for the round
    public void SwitchToEvaluation(List<Card> teamCards, List<Card> enemyCard, int teamHP, int teamShield, int enemyHP)
    {
        SwitchState(BattleState.EvaluateRound);

        DisplayPlayerCards(teamCards);
        DisplayEnemyCards(enemyCard);
        UpdateStats(teamHP, teamShield);

        bossHpBar.value = enemyHP;
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
            c.InitiateCardDisplay(draw[i]);

            teamCardDisplay.Add(card);
        }
    }

    #endregion

    #region internal methods

    // dont call this on its own
    private void DisplayPlayerCards(List<Card> draw) {
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
            c.InitiateCardDisplay(draw[i]);
            c.InitiateSelectableCard(cardSelectors, i); // erste variable sind die ICardSelector an die die response geschickt wird wenn eine karte geclickt wird

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
            c.InitiateCardDisplay(draw[i]);

            enemyCardDisplay.Add(card);
        }
    }


    private void UpdateStats(int teamHP, int teamShield) {
        healthLabel.text = "HP: " + teamHP;
        shieldLabel.text = "Shield: " + teamShield;
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
    Rewards
}