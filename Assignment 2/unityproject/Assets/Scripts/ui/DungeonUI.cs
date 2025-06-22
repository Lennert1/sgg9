using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonUI : UI
{
    [SerializeField] private GameObject lobby;
    [SerializeField] private List<GameObject> miniGames;
    [Space]
    [SerializeField] private Button startGameButton;


    private GameObject loadedGame;
    private Dungeon dungeon;

    public override void SetActive(bool b)
    {
        base.SetActive(b);

        if (b) { lobby.SetActive(true);

#warning missing: REST-Call to retrieve dungeon data
            dungeon = new Dungeon(new List<Character>() { new Character(characterType.Knight) });


            startGameButton.GetComponent<Image>().color = (GameManager.Instance.usrData.pid == 0) ? Color.grey : Color.green;
            startGameButton.enabled = GameManager.Instance.usrData.pid != 0;
        }
    }

    public override void Unload()
    {
        loadedGame.Destroy();
        loadedGame = null;

        base.Unload();
    }

    public void PressStartButton() {
        if (loadedGame != null)
        {
            LoadMiniGame(); return;
        }
        
        /* missing behavior

        check for if party members ahve entered and listen for web sockets calls that others enter

        */

        LoadMiniGame();
    }

    public void LoadMiniGame() {
        bool n;
        if (n = loadedGame == null)
        {
            if (dungeon.miniGameType > miniGames.Count) Debug.LogError("MiniGame of Type: (" + dungeon.miniGameType + ") doesn't exist!");
            loadedGame = Instantiate(miniGames[dungeon.miniGameType], transform);
        }

        lobby.SetActive(false);

        loadedGame.SetActive(true);

        if (!n) return;
        loadedGame.GetComponent<MiniGame>().InitiateMiniGame(this, dungeon);
    }

    public void ExitMiniGame() {
        loadedGame.SetActive(false);
        lobby.SetActive(true);
    }
}