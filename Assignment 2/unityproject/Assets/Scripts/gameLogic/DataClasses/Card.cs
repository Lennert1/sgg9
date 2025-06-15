using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
