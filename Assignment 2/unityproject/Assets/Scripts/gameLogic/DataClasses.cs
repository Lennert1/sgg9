using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using gamelogic.ServerClasses;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

[Serializable]
public class Card
{
    public int type;
    public int lvl = 1;
    public int count = 0;

    // ========== //

    public Card(int type, int lvl = 1, int count = 1)
    {
        this.type = type;
        this.lvl = lvl;
        this.count = count;
    }
}

[Serializable]
public class User
{
    public int uid;

    // party ID: 0 := not member of a party
    public int pid = 0;
    public string name;
    public int lvl = 1;
    public int gold;
    public int armorpoints;
    public List<Card> cards;
    public List<Character> characters;

    // ========== //

    [JsonConstructor]
    public User(int uid, string name)
    {
        this.uid = uid;
        this.name = name;

        cards = new List<Card>();
        characters = new List<Character>();
    }

    public User(int uid, string name, List<Card> cards, List<Character> characters)
    {
        this.uid = uid;
        this.name = name;

        this.cards = cards;
        this.characters = characters;
    }
}

[Serializable]
public class Party
{
    public int pid;
    public int memberCount
    {
        get { return members.Count; }
    }
    public int hp;
    public int shield;
    public List<int> members;

    [JsonConstructor]
    public Party(User leader)
    {
        members = new List<int>();
        members.Add(leader.uid);
        hp = leader.armorpoints;
    }
}

public class Character
{
    public enum Type
    {
        Knight,
        Shaman,
        Wizard
    }

    public Type type;
    public int xp;
    public int lvl;
    public int hp;
    public List<Card> deck;

    //Kartenlimit ist denke ich sinnvoll
    private int deckLimit = 10;

    // ========== //

    public Character(Type type)
    {
        this.type = type;
        xp = 0;
        lvl = 1;
        deck = new List<Card>();

        //Knight hat mehr Hp
        if (type == Type.Knight)
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

class Enemy
{
    public int lvl;
    public int maxHp;
    public int hp;
    public List<Card> deck;


    public Enemy(int lvl, List<Card> deck)
    {
        this.lvl = lvl;
        maxHp = lvl * 100;
        hp = maxHp;
        if (deck == null || deck.Count == 0)
        {
            CreateDeck();
        }
        else
        {
            this.deck = deck;
        }
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
}

class Boss : Enemy
{
    
    public Boss(int lvl) : base(lvl, CreateBossDeck(lvl))
    {
        maxHp = lvl * 120;
        hp = maxHp;
    }

    public Boss(int lvl, List<Card> deck) : base(lvl, deck)
    {
        maxHp = lvl * 120;
        hp = maxHp;
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
