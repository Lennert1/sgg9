using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    
    public Boss(int lvl) : base(lvl, CreateBossDeck(lvl))
    {
        hp = lvl * 120;
    }

    public Boss(int lvl, List<Card> deck) : base(lvl, deck)
    {
        hp = lvl * 120;
    }

    //bisher auch hier: Leeres Deck -> 1 Karte von Typ 0
    private static List<Card> CreateBossDeck(int lvl)
    {
        List<Card> deck = new List<Card>();
        deck.Add(new Card(0, lvl, 1));
        return deck;
    }
    
    public override Card action()
    {
        return base.action();
    }
}
