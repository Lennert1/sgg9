using System.Collections;
using System.Collections.Generic;
using Mapbox.Json;
using TMPro;
using UnityEngine;

public class BattleArenaUI : MiniGameUI, ICardSelector
{
    #region general

    private BattleArena data;
    private BattleState state;

    [SerializeField] private GameObject InitialDisplay;
    [SerializeField] private Transform playerCardDisplayCenter;
    [SerializeField] private Transform enemyCardDisplayCenter;
    [SerializeField] private Transform teamCardDisplayCenter;
    [SerializeField] private GameObject teamStatsDisplay;

    #endregion

    #region cardDisplays

    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Vector2 positionOffset;
    [SerializeField] private float teamCardDisplayScaleFactor;

    private List<GameObject> playerCardDisplay = new List<GameObject>();
    private List<GameObject> enemyCardDisplay = new List<GameObject>();
    private List<GameObject> teamCardDisplay = new List<GameObject>();

    List<ICardSelector> cardSelectors;

    #endregion

    [SerializeField] private TextMeshProUGUI healthLabel;
    [SerializeField] private TextMeshProUGUI shieldLabel;


    public override void InitiateMiniGame(DungeonUI ui, Dungeon d)
    {
        base.InitiateMiniGame(ui, d);

        cardSelectors = new List<ICardSelector> { this };

        data = JsonConvert.DeserializeObject<BattleArena>(d.miniGameJson);

        SwitchState(BattleState.Initial);
    }

    public void SelectCard(int p)
    {
        Debug.Log("Selected Card: " + p);
    }

    #region control methods

    public void SwitchState(BattleState s)
    {
        state = s;

        switch (s)
        {
            case BattleState.Initial:
                {

                    playerCardDisplayCenter.gameObject.SetActive(false);
                    enemyCardDisplayCenter.gameObject.SetActive(false);
                    teamCardDisplayCenter.gameObject.SetActive(false);
                    SwitchToCardSelector(new List<Card> { new Card(1, 1, 1), new Card(2, 2, 2), new Card(3, 3, 3), new Card(4, 4, 4) }, 100, 20);
#warning To Be Replaced: wait for game logic to initiate first round
                    break;
                }
            case BattleState.SelectCard:
                {
                    playerCardDisplayCenter.gameObject.SetActive(true);
                    enemyCardDisplayCenter.gameObject.SetActive(true);
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
    public void SwitchToCardSelector(List<Card> draw, int cHP, int cShield)
    {
        SwitchState(BattleState.SelectCard);

        DisplayPlayerCards(draw);
        UpdateStats(cHP, cShield);
    }

    //call this to begin evaluation after all members have selected their card wfor the round
    public void SwitchToEvaluation(List<Card> teamCards, List<Card> enemyCard, int cHP, int cShield) {
        SwitchState(BattleState.EvaluateRound);

        DisplayPlayerCards(teamCards);
        DisplayEnemyCards(enemyCard);
        UpdateStats(cHP, cShield);
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
    public void DisplayPlayerCards(List<Card> draw) {
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
    public void DisplayEnemyCards(List<Card> draw) {
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


    private void UpdateStats(int cHP, int cShield) {
        healthLabel.text = "HP: " + cHP;
        shieldLabel.text = "Shield: " + cShield;
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