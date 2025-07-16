using System;
using System.Collections;
using System.Collections.Generic;
using Mapbox.Json;
using UnityEngine;

public class BattleArena
{
    public Enemy enemy;
    public int rewardXp;
    public int rewardGold;
    public List<Card> rewardCards;
    public int rewardArmorpoints;
    private System.Random rnd = new System.Random();

    
    public BattleArena(List<Character> characters)
    {
        int averageLvl = 0;
        for (int i = 0; i < characters.Count; i++)
        {
            averageLvl += characters[i].lvl;
        }
        averageLvl = averageLvl / characters.Count;
        
        enemy = new Enemy(averageLvl);
        
        
        rewardXp = calculateXP(averageLvl);
        
        rewardGold = calculateGold(averageLvl);
        
        rewardCards = calculateCards(averageLvl);
        rewardArmorpoints = calculateArmorpoints(averageLvl);
    }

    private int calculateGold(int averageLvl)
    {
        //20 faches Durchschnittlevel +- 10%
        return (int) (averageLvl * 20 * (rnd.NextDouble() * 0.2 + 0.9));
    }
    
    private int calculateXP(int averageLvl)
    {
        //1/4 der benötigten xp des durchschnittlevels der gruppe +- 10%
        return (int) (100 * Math.Pow(1.5, averageLvl - 1) / 4 * (rnd.NextDouble() * 0.2 + 0.9));
    }
    
    private List<Card> calculateCards(int averageLvl)
    {
        //Random Karte * durchschnittslevel und 10% chance auf zweite karte
        if (GameManager.Instance.RewardPool.Count == 0)
        {
            Debug.LogWarning("No Reward Pool Available");
            return new List<Card>{new Card(0,0,averageLvl)};
        }
        
        List<Card> cards = new List<Card>();
        int i = rnd.Next(0, GameManager.Instance.RewardPool.Count);
        cards.Add(GameManager.Instance.RewardPool[i]);
        cards[0].count = averageLvl;

        if (rnd.NextDouble() < 0.1)
        {
            int j = rnd.Next(0, GameManager.Instance.RewardPool.Count - 1);
            if (j >= i) j++;
            cards.Add(GameManager.Instance.RewardPool[j]);
            cards[1].count = averageLvl;
        }
        return cards;
    }
    
    private int calculateArmorpoints(int averageLvl)
    {
        return averageLvl * 50;
    }

    [JsonConstructor]
    public BattleArena()
    {
        
    }
}
