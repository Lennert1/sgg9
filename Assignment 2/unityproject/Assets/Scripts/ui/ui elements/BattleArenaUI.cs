using System.Collections;
using System.Collections.Generic;
using Mapbox.Json;
using UnityEngine;

public class BattleArenaUI : MiniGameUI
{
    private BattleArena data;

    public override void InitiateMiniGame(DungeonUI ui, Dungeon d)
    {
        base.InitiateMiniGame(ui, d);

        data = JsonConvert.DeserializeObject<BattleArena>(d.miniGameJson);
    }
}
