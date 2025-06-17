using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

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
