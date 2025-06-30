using System.Collections;
using System.Collections.Generic;
using Mapbox.Json;
using UnityEngine;

public class BattleArenaUI : MiniGameUI, ICardSelector
{
    #region general

    private BattleArena data;
    private BattleState state;

    [SerializeField] List<GameObject> displays;

    #endregion

    #region select card

    [SerializeField] private Transform cardDisplayCenter;
    [SerializeField] private int xPositionOffset;
    [SerializeField] private GameObject cardPrefab;

    private List<GameObject> cardDisplay;

    #endregion


    public override void InitiateMiniGame(DungeonUI ui, Dungeon d)
    {
        base.InitiateMiniGame(ui, d);

        data = JsonConvert.DeserializeObject<BattleArena>(d.miniGameJson);

        SwitchState(BattleState.Initial);
    }

    public void SelectCard(int p)
    {
        Debug.Log("Selected Card: " + p);
    }

    public void SwitchState(BattleState s)
    {
        state = s;

        switch (s)
        {
            case BattleState.Initial:
                {
                    foreach (GameObject d in displays) d.SetActive(false);
                    SwitchToCardSelector(new List<Card> { new Card(1, 1, 1), new Card(2, 2, 2), new Card(3, 3, 3) });
#warning To Be Replaced: wait for game logic to initiate first round
                    break;
                }
            case BattleState.SelectCard:
                {
                    displays[1].SetActive(true);
                    break;
                }
            case BattleState.EvaluateRound:
                {
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

    public void SwitchToCardSelector(List<Card> draw)
    {
        SwitchState(BattleState.SelectCard);

        cardDisplay = new List<GameObject>();
        for (int i = 0; i < draw.Count; i++)
        {
            int xOffset = i * xPositionOffset - ((draw.Count - 1) * xPositionOffset / 2);
            Vector3 pos = new Vector3(xOffset, 0);

            GameObject card = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity, cardDisplayCenter);
            card.transform.localPosition = pos;
            CardDisplay c = card.GetComponent<CardDisplay>();
            c.InitiateCardDisplay(draw[i]);
            c.InitiateSelectableCard(this, i); // erste variable ist das ICardSelector an das die response geschickt wird wenn eine karte geclickt wird

            cardDisplay.Add(card);
        }
    }
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