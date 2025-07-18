using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class User
{
    public string uid;

    // party ID: 0 := not member of a party
    public int pid = 0;
    public string name;
    public int lvl {get
        {
            int v = 0;
            foreach (Character c in characters) v += c.lvl;
            return v;
        }
    }
    public int gold;
    public int upgradePoints;
    public List<Card> cards;
    public List<Character> characters;
    public List<string> friendsUID;
    public int selectedCharacter;

    // ========== //

    [JsonConstructor]
    public User(string uid, string name)
    {
        this.uid = uid;
        this.name = name;

        cards = new List<Card>();
        characters = new List<Character>()
        {
            new Character(characterType.Assassin),
            new Character(characterType.Paladin),
            new Character(characterType.Shaman)
        };
        friendsUID = new List<string>();
    }

    public User(string uid, string name, List<Card> cards, List<Character> characters, List<string> friendsUID)
    {
        this.uid = uid;
        this.name = name;

        this.cards = cards;
        this.characters = characters;
        this.friendsUID = friendsUID;

    }
}
