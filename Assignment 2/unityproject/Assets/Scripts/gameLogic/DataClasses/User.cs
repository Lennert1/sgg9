using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [JsonConstructor]
    public User(string uid, string name, int gold, int upgradePoints, int selectedCharacter, List<Card> cards, List<Character> characters, List<string> friendsUID)
    {
        this.uid = uid;
        this.name = name;
        this.gold = gold;
        this.upgradePoints = upgradePoints;
        this.selectedCharacter = selectedCharacter;
        this.cards = cards ?? new List<Card>();
        this.characters = characters ?? new List<Character>();
        this.friendsUID = friendsUID ?? new List<string>();
    }

    public override string ToString()
    {
        string charactersStr = string.Join(", ", characters.Select(c => c.ToString()));
        string cardsStr = string.Join(", ", cards.Select(card => card.ToString()));
        string friendsStr = string.Join(", ", friendsUID);

        return $"User {{ uid: {uid}, name: {name}, gold: {gold}, upgradePoints: {upgradePoints}, selectedCharacter: {selectedCharacter}, " +
               $"cards: [{cardsStr}], characters: [{charactersStr}], friendsUID: [{friendsStr}] }}";
    }
}
