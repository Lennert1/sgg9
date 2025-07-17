using System;
using System.Collections;
using System.Collections.Generic;
using Mapbox.Json;
using UnityEngine;

[Serializable]
public class BattleArena
{
    public int partyID;

    [Space]

    public Enemy enemy;
    public int rewardGold;
    public List<Card> rewardCards = new();
    public int rewardUpgradePoints;

    [Space]
    public List<bool> playerChecks = new();
    public List<Card> playerCards = new();
    public List<Card> enemyCards = new();
    public BattleState battleState;


    public BattleArena(List<Character> characters, Enemy enemy)
    {
        int averageLvl = 0;
        for (int i = 0; i < characters.Count; i++)
        {
            averageLvl += characters[i].lvl;
        }
        averageLvl = averageLvl / characters.Count;

        this.enemy = enemy;

        rewardGold = (int)((0.1 * averageLvl + 0.9) * 25);

        rewardCards = calculateCards(averageLvl);
        rewardUpgradePoints = (int)((0.1 * averageLvl + 0.9) * 10);
    }

    private List<Card> calculateCards(int averageLvl)
    {
        System.Random rnd = new();

        //Random Karte * durchschnittslevel und 10% chance auf zweite karte
        if (GameManager.Instance.RewardPool.Count == 0)
        {
            Debug.LogWarning("No Reward Pool Available");
            return new();
        }

        List<Card> cards = new List<Card>();
        int i = rnd.Next(0, GameManager.Instance.RewardPool.Count);
        cards.Add(new Card(GameManager.Instance.RewardPool[i].type, count: averageLvl));

        if (rnd.NextDouble() < 0.1)
        {
            int j = rnd.Next(0, GameManager.Instance.RewardPool.Count - 1);
            if (j >= i) j++;
            cards.Add(new Card(GameManager.Instance.RewardPool[j].type, count: averageLvl));
            cards[1].count = averageLvl;
        }
        return cards;
    }

    [JsonConstructor]
    public BattleArena()
    {

    }
}
