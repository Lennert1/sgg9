using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class Dungeon
{
    public int miniGameType = 0;
    public string miniGameJson = "";

    public Dungeon(int miniGameType)
    {
        this.miniGameType = miniGameType;
    }
}
