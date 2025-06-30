using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DungeonUI : UI
{
    [SerializeField] private GameObject lobby;
    [SerializeField] private List<GameObject> miniGames;
    [Space]
    [SerializeField] private Button startGameButton;


    private GameObject loadedGame;
    private int poiID;
    private Dungeon dungeon;
    private bool waitToEnter;

    public override void SetActive(bool b)
    {
        base.SetActive(b);

        if (b) {
            lobby.SetActive(true);

            poiID = GameManager.Instance.currentPoiID;

#warning missing: REST-Call to retrieve dungeon data (mit GameManager.currentPOIid)
            dungeon = new Dungeon(new List<Character>() { new Character(characterType.Knight) });


            startGameButton.GetComponent<Image>().color = (GameManager.Instance.usrData.pid == 0) ? Color.grey : Color.green;
            startGameButton.enabled = GameManager.Instance.usrData.pid != 0;
            if (GameManager.Instance.usrData.pid != 0) GameManager.Instance.LoadPartyData();
        }
        else {
#warning missing: Web-Socket-Call that player exited POI
        }
    }

    public override void Unload()
    {
        loadedGame.Destroy();
        loadedGame = null;

        base.Unload();
    }

    public void PressStartButton() {
        if (waitToEnter) return;

        if (loadedGame != null)
        {
            LoadMiniGame(); return;
        }

        bool allEntered = true;
        foreach (int id in GameManager.Instance.usrParty.memberPoIids)
        {
            if (id != poiID)
            {
                allEntered = false;
                break;
            }
        }

        if (allEntered)
        {
            LoadMiniGame();
            return;
        }

        waitToEnter = true;
        startGameButton.GetComponent<Image>().color = Color.grey;
        startGameButton.GetComponentInChildren<TextMeshProUGUI>().text = "waiting...";

        /*
            warten auf web socket calls über updates an den pois der anderen spieler und automatisch das minigame betreten wenn alle bereit sind
        */
    }

    public void LoadMiniGame() {
        bool n;
        if (n = loadedGame == null)
        {
            if (dungeon.miniGameType > miniGames.Count) Debug.LogError("MiniGame of Type: (" + dungeon.miniGameType + ") doesn't exist!");
            loadedGame = Instantiate(miniGames[dungeon.miniGameType], transform);
        }

        startGameButton.GetComponent<Image>().color = Color.blue;
        startGameButton.GetComponentInChildren<TextMeshProUGUI>().text = "Enter";
        lobby.SetActive(false);

        loadedGame.SetActive(true);

        if (!n) return;
        loadedGame.GetComponent<MiniGameUI>().InitiateMiniGame(this, dungeon);
    }

    public void ExitMiniGame() {
        loadedGame.SetActive(false);
        lobby.SetActive(true);
    }
}