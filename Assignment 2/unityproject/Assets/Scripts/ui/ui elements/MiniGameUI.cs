using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MiniGameUI : MonoBehaviour
{
    protected DungeonUI dungeonUI;
    protected Dungeon dungeon;

    public virtual void InitiateMiniGame(DungeonUI ui, Dungeon d)
    {
        dungeonUI = ui;
        dungeon = d;
    }

    public virtual void LeaveMiniGame()
    {
        dungeonUI.ExitMiniGame();
    }
}
