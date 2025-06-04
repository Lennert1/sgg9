using System;
using System.Collections;
using System.Collections.Generic;
using gamelogic.ServerClasses;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UIElements;

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
    public string name;
    public int lvl = 1;
    public int gold;
    public int armorpoints;
    public List<Card> cards;

    // ========== //

    public User(int uid, string name)
    {
        this.uid = uid;
        this.name = name;

        cards = new List<Card>();
    }

    public User(int uid, string name, List<Card> cards)
    {
        this.uid = uid;
        this.name = name;

        this.cards = cards;
    }
}
