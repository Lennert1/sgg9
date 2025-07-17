using System;
using System.Collections;
using System.Collections.Generic;
using Mapbox.Json;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Character
{

    public characterType type;
    public int xp;
    public int lvl;
    private int baseHP;
    public int hp;
    public List<Card> deck;

    //Kartenlimit ist denke ich sinnvoll
    private int deckLimit = 8;

    // ========== //

    public Character(characterType type)
    {
        this.type = type;
        xp = 0;
        deck = new List<Card>();


        switch (type)
        {
            case characterType.Assassin:
                {
                    baseHP = 70;
                    deck = new List<Card> { new Card(1), new Card(2), new Card(2), new Card(4) };
                    break;
                }
            case characterType.Paladin:
                {
                    baseHP = 240;
                    deck = new List<Card> { new Card(2), new Card(2), new Card(5), new Card(5) };
                    break;
                }
            case characterType.Shaman:
                {
                    baseHP = 140;
                    deck = new List<Card> { new Card(1), new Card(3), new Card(3), new Card(3) };
                    break;
                }
            case characterType.Wizard:
                {
                    baseHP = 1;
                    break;
                }
            default: break;
        }

        SetLevel(1);
    }

    [JsonConstructor]
    public Character()
    {
        deck = new();
    }

    public void SetLevel(int lvl)
    {
        this.lvl = lvl;
        hp = (int)(baseHP * Math.Pow(1.2, (double)lvl - 1));
    }

    public int GetRequiredUpgradePoints()
    {
        return lvl * lvl + 4;
    }
}

public enum characterType
{
    Assassin,
    Paladin,
    Shaman,
    Wizard
}
