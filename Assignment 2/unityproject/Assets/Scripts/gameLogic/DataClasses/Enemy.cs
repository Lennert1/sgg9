using System.Collections;
using System.Collections.Generic;
using Mapbox.Json;
using UnityEngine;

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
    public Enemy(int type, int lvl = 1)
    {
        enemyType = type;
        hp = lvl * 100;
        CreateDeck();
        this.lvl = lvl;
    }

    public void CreateDeck()
    {
        deck = new();
    }
    
    [JsonConstructor]
    public Enemy()
    {
        
    }
}
