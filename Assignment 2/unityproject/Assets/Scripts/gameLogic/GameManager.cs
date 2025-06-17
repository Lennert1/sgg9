using System.Collections;
using System.Collections.Generic;
using gamelogic.ServerClasses;
using Mapbox.Json;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
        private set
        {
            instance = value;
        }
    }
    private static GameManager instance;


    #region fields

    [SerializeField] private UI activeUI;
    [SerializeField] private UI[] allUIs;


    public User usrData;
    public Party usrParty;

    // == 0 if not entered any PoI
    public int currentPoiID;

    #endregion

    #region onAwake

    void Awake()
    {
        if (instance != null) Debug.LogError("Multiple Instances of GameManager!");
        else instance = this;

        LoadUserData();

        foreach (UI u in allUIs)
        {
            u.SetActive(false);
        }
        activeUI.SetActive(true);
    }

    #endregion

    #region game logic

    public void SetUIActive(UI ui)
    {
        activeUI.Unload();

        ui.SetActive(true);
        activeUI = ui;
    }
    
    public void StartBattle(Dungeon dungeon)
    {
        // ui stuff
        BattleManager.Instance.StartBattle(usrParty, dungeon);
    }

    #endregion

    #region data management

    public void LoadUserData()
    {
        // for testing purposes only:
        usrData = new User(1234, "Pony", new List<Card>() { new Card(16, 1, 1), new Card(1, 16, 1), new Card(1, 1, 16), new Card(1, 1, 1), new Card(16, 16, 16), new Card(16, 1, 16), new Card(1, 16, 16), new Card(16, 16, 1), new Card(16, 1, 1), new Card(1, 1, 16) }, new List<Character>());

#warning missing: REST-Call to retrieve user data
        //RestServerCaller.Instance.GenericRequestCall("", UserCallback);
    }

    public void SaveUserData()
    {
#warning missing: REST-Call to send user data
        //RestServerCaller.Instance.GenericSendCall("", usrData);
    }

    public void LoadPartyData()
    {
        // for testing purposes only:
        usrParty = new Party(usrData);

#warning missing: REST-Call to retrieve user data
        //RestServerCaller.Instance.GenericRequestCall("", PartyCallback);
    }

    #endregion

    #region Callback Functions

    public void UserCallback(ServerMessage response)
    {
        if (response.IsError())
        {
            Debug.Log("Error");
            return;
        }

        usrData = JsonConvert.DeserializeObject<User>(response.message);
    }
    
    public void PartyCallback(ServerMessage response)
    {
        if (response.IsError())
        {
            Debug.Log("Error");
            return;
        }

        usrParty = JsonConvert.DeserializeObject<Party>(response.message);
    }

    #endregion
}
