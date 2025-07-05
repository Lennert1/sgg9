using System;
using System.Collections;
using System.Collections.Generic;
using Mapbox.Json;
using UnityEngine;

public class BattleArena
{
    public Boss boss;
    public int rewardXp;
    public int rewardGold;
    public Card rewardCard;
    public int rewardArmorpoints;

    
    public BattleArena(List<Character> characters)
    {
        int averageLvl = 0;
        for (int i = 0; i < characters.Count; i++)
        {
            averageLvl += characters[i].lvl;
        }
        averageLvl = averageLvl / characters.Count;
        
        boss = new Boss(averageLvl);
        
        System.Random rnd = new System.Random();
        
        //1/4 der benötigten xp des durchschnittlevels der gruppe +- 10%
        rewardXp = (int) (100 * Math.Pow(1.5, averageLvl - 1) / 4 * (rnd.NextDouble() * 0.2 + 0.9));
        
        //20 faches Durchschnittlevel +- 10%
        rewardGold = (int) (averageLvl * 20 * (rnd.NextDouble() * 0.2 + 0.9));
        
        rewardCard = new Card(0, 0, averageLvl);
        rewardArmorpoints = averageLvl * 50;
    }

    [JsonConstructor]
    public BattleArena()
    {
        
    }
}
