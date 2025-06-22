using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MiniGame : MonoBehaviour
{
    private DungeonUI dungeonUI;
    private Dungeon dungeon;

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
