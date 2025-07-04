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
    private Dungeon dungeon;
    private bool waitToEnter;

    public override void SetActive(bool b)
    {
        base.SetActive(b);

        if (b) {
            lobby.SetActive(true);

#warning dungeon = BattleManager.Instance.dungeon;


            startGameButton.GetComponent<Image>().color = (GameManager.Instance.usrData.pid == 0) ? Color.grey : Color.green;
            startGameButton.GetComponentInChildren<TextMeshProUGUI>().text = "Ready!";
            startGameButton.enabled = GameManager.Instance.usrData.pid != 0;
            if (GameManager.Instance.usrData.pid != 0) GameManager.Instance.LoadPartyData();
        }
        else {
#warning BattleManager.Instance.unready()
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

        waitToEnter = true;
        startGameButton.GetComponent<Image>().color = Color.grey;
        startGameButton.GetComponentInChildren<TextMeshProUGUI>().text = "waiting...";

#warning BattleManager.Instance.ready()
    }

    public void AllPlayersReady() {
        waitToEnter = false;
        LoadMiniGame();
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