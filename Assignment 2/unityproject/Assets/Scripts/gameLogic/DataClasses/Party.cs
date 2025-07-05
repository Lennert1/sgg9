using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

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
    public List<int> memberPoIids;

    [JsonConstructor]
    public Party(User leader)
    {
        members = new List<int> { leader.uid };
        memberPoIids = new List<int>();
        hp = leader.characters[0].hp;
        shield = leader.armorpoints;
    }
}
