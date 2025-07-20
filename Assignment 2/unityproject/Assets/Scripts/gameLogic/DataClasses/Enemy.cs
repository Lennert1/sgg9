using System;
using System.Collections;
using System.Collections.Generic;
using Mapbox.Json;
using UnityEngine;

[Serializable]
public class Enemy
{
    public int enemyType;
    public int lvl;
    public int hp;
    public List<Card> deck;


    public Enemy(int type, List<Card> deck, int hp, int lvl = 1)
    {
        enemyType = type;
        this.deck = deck;
        this.lvl = lvl;
        this.hp = (int)((0.9 + (0.1 * lvl)) * hp);
    }

    [JsonConstructor]
    public Enemy()
    {

    }
}
