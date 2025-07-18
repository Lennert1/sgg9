using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.VisualScripting;
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
    public Party(int pid, List<int> members, List<int> memberPoIids)
    {
        this.pid = pid;
        this.members = members;
        this.memberPoIids = memberPoIids;
    }
    public Party(User leader)
    {
        members = new List<int> { leader.uid };
        memberPoIids = new List<int> {0};
        hp = leader.characters[0].hp;
        shield = leader.upgradePoints;
    }

    public override string ToString()
    {
        string membersList = members != null ? string.Join(", ", members) : "None";
        string memberPoIidsList = memberPoIids != null ? string.Join(", ", memberPoIids) : "None";

        return $"Party Details:\n" +
               $"- PID: {pid}\n" +
               $"- HP: {hp}\n" +
               $"- Shield: {shield}\n" +
               $"- Member Count: {memberCount}\n" +
               $"- Members: {membersList}\n" +
               $"- MemberPoIids: {memberPoIidsList}";
    }



}
