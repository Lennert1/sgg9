using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{

    public characterType type;
    public int xp;
    public int lvl;
    public int hp;
    public List<Card> deck;

    //Kartenlimit ist denke ich sinnvoll
    private int deckLimit = 10;

    // ========== //

    public Character(characterType type)
    {
        this.type = type;
        xp = 0;
        lvl = 1;
        deck = new List<Card>();

        //Knight hat mehr Hp
        if (type == characterType.Knight)
        {
            hp = 150;
        }
        else
        {
            hp = 100;
        }
    }

    public void AddCardToDeck(Card card)
    {
        if (deck.Count < deckLimit && !deck.Contains(card))
        {
            deck.Add(card);
        }
    }

    public void RemoveCardFromDeck(Card card)
    {
        deck.Remove(card);
    }
    private void CheckLvlUp()
    {
        while (true)
        {
            //Benötigte Xp *1,5 pro Level, startet bei 100xp
            if (xp >= (int)(100 * Math.Pow(1.5, lvl - 1)))
            {
                lvl++;
                xp -= (int)(100 * Math.Pow(1.5, lvl - 1));

                /* LevelUp boni hierhin
                 
                hp += lvl * 20
                
                */

                continue;
            }

            break;
        }
    }

    public void AddXp(int rewardXp)
    {
        this.xp += rewardXp;
        CheckLvlUp();
    }
}

public enum characterType
{
    Knight,
    Shaman,
    Wizard
}
