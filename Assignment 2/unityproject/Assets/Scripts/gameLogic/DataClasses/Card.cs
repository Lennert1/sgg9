using Mapbox.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Card
{
    public int type; // 0 := backside of a card
    public int lvl = 1;
    public int count = 0;

    // ========== //

    [JsonConstructor]
    public Card(int type, int lvl, int count)
    {
        this.type = type;
        this.lvl = lvl;
        this.count = count;
    }    

    public int RequiredCardsForUpgrade()
    {
        return (int)(0.2 * lvl * lvl + 10);
    }

    public void addCards(int i)
    {
        count += i;
    }

    public override string ToString()
    {
        return $"Card {{ type: {type}, lvl: {lvl}, count: {count} }}";
    }
}
