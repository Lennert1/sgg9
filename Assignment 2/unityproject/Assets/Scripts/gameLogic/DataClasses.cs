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

    public Card(int type)
    {
        this.type = type;
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
    public List<Card> inventory;

    // ========== //

    public User(int uid, string name)
    {
        this.uid = uid;
        this.name = name;

        inventory = new List<Card>();

        inventory.Add(new Card(23));
        inventory.Add(new Card(7));
        inventory.Add(new Card(320));
    }

    // ========== //

    public RestServerCaller.ServerRequestCallBack callback()
    {
        return (ServerMessage serverMessage) =>
        {
            if (serverMessage.IsError())
            {
                Debug.Log("Error");
                return;
            }
            var serverData = JsonConvert.DeserializeObject<User>(serverMessage.message);
        };
    }
}
