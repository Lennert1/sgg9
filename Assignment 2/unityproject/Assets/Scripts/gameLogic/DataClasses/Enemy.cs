using System.Collections;
using System.Collections.Generic;
using Mapbox.Json;
using UnityEngine;

public class Enemy
{
    public int lvl;
    public int hp;
    public List<Card> deck;
    public int enemyType;


    public Enemy(int lvl, List<Card> deck)
    {
        this.lvl = lvl;
        hp = lvl * 100;
        if (deck == null || deck.Count == 0)
        {
            CreateDeck();
        }
        else
        {
            this.deck = deck;
        }
    }
    public Enemy(int lvl)
    {
        this.lvl = lvl;
        hp = lvl * 100;
        CreateDeck();
    }

    //Leeres Deck -> 1 Karte von Typ 0
    public void CreateDeck()
    {
        this.deck = new List<Card>();
        this.deck.Add(new Card(0, lvl, 1));
    }
    public virtual Card action()
    {
        return deck[new System.Random().Next(deck.Count)];
    }
    
    [JsonConstructor]
    public Enemy()
    {
        
    }
}
