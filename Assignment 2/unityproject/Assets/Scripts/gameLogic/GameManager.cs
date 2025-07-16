using System;
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

    private API _api;

    public API GetAPI() {
        return _api;
    }



    #region fields

    /* allUIs Indices:
    0: LoginUI
    1: MapUI
    2: Profile
    3: Inventory
    4: CharacterMenu
    5: TavernUI
    6: DungeonUI
    */

    [SerializeField] private UI activeUI;
    [SerializeField] public UI[] allUIs;
    private MiniGameUI activeMiniGameUI;
    
    public User usrData;
    public Party usrParty;

    // == 0 if not entered any PoI
    // Assigning eventIDs with following system: 1 to 99 == dungeon, 100 to 199 == taverns, 200 to 299 == shops
    public int currentPoiID;

    [SerializeField] private List<int> rewardPool;
    public List<Card> RewardPool { get; private set; }

    #endregion

    #region data memory

    private User tUser;
    private User loadedUser;

    #endregion

    #region onAwake

    void Awake()
    {
        if (instance != null) Debug.LogError("Multiple Instances of GameManager!");
        else instance = this;

        _api = GameObject.Find("UI").GetComponent<API>();

        // User data is loaded locally when a login happens
        LoadUserData();

        List<Card> r = new();
        foreach (int i in rewardPool)
        {
            r.Add(new Card(i));
        }
        RewardPool = r;

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

    public void SetActiveMiniGameUI(MiniGameUI m) {
        activeMiniGameUI = m;
    }

    public void UpdateCardDeckLevels()
    {
        for (int c = 0; c < usrData.characters.Count; c++)
        {
            for (int d = 0; d < usrData.characters[c].deck.Count; d++)
            {
                for (int i = 0; i < usrData.cards.Count; i++)
                {
                    if (usrData.characters[c].deck[d].type == usrData.cards[i].type)
                    {
                        usrData.characters[c].deck[d].lvl = usrData.cards[i].lvl; break;
                    }
                }
            }
        }
    }

    public void AddCardsToInventory(List<Card> cards)
    {
        foreach (Card c in cards)
        {
            bool a = false;
            for (int i = 0; i < usrData.cards.Count; i++)
            {
                if (usrData.cards[i].type == c.type)
                {
                    a = true;
                    usrData.cards[i].count += c.count;
                    if (usrData.cards[i].count >= usrData.cards[i].RequiredCardsForUpgrade())
                    {
                        usrData.cards[i].count -= usrData.cards[i].RequiredCardsForUpgrade();
                        usrData.cards[i].lvl += 1;
                    }
                }
            }
            if (!a) usrData.cards.Add(c);
        }
    }

    #endregion

    #region data management

    // load User of this device, this function should be used to update any change on the usrData in the gameManager
    public void LoadUserData()
    {
        //for testing purposes only:
        usrData = new User(1234, "Pony");
        if (GameAssetManager.Instance != null) usrData.cards = GameAssetManager.Instance.CreateInventoryOfAllCards();
        usrData.pid = 1;
        usrData.gold = 2000;

        return;

        // This function loads the user data from json as C# object so you can access the data 
        if (_api != null)
        {
            usrData = _api.LoadUserDataFromFile();
        }
    }

    public void UpdateUserData(User user)
    {
        _api.SaveUserDataToFile(user);
    }

    // load any User by their ID
    public User LoadUserData(int id) 
    {
        return new User(id, "" + id);

        // Correct method
        /*RestServerCaller.Instance.GetUserByIdRequestCall(id, SetLoadedUser);
        return loadedUser;*/
    }

    /*public void SaveUserData()
    {
#warning missing: REST-Call to send user data
        //RestServerCaller.Instance.GenericSendCall("", usrData);
    } */

    public void LoadPartyData()
    {
        // for testing purposes only:
        usrParty = new Party(usrData);
        //usrParty.members.Add(22769834);
        //usrParty.members.Add(8976);
        //usrParty.memberPoIids.Add(123);

        // Correct method
        /*string url = "http://127.0.0.1:8000/api/partyById/" + usrData.pid + "/";
        RestServerCaller.Instance.GenericRequestCall(url, PartyCallback);*/
    }

    #endregion

    #region Callback Functions

    public void ThisUserCallback(ServerMessage response)
    {
        if (response.IsError())
        {
            Debug.Log("Error");
            return;
        }

        usrData = JsonConvert.DeserializeObject<User>(response.message);
    }

    public void UserCallbackByID(ServerMessage response)
    {
        if (response.IsError())
        {
            Debug.Log("Error");
            return;
        }

        tUser = JsonConvert.DeserializeObject<User>(response.message);
    }
    
    public void PartyCallback(ServerMessage response)
    {
        if (response.IsError())
        {
            Debug.Log("Error");
            return;
        }

        usrParty = JsonUtility.FromJson<Party>(response.message);
        usrParty.initializeParty();
        //Debug.Log(usrParty);
    }

    public void SetLoadedUser(User user)
    {
        if (user == null)
        {
            Debug.Log("User could not been found!");
            return;
        }
        loadedUser = user;
    }

    #endregion
}
