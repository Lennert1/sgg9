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


    #region fields

    /* allUIs Indices:
    0: MapUI
    1: LoginUI
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
    public Party partyData;

    // == 0 if not entered any PoI
    // Assigning eventIDs with following system: 1 to 99 == dungeon, 100 to 199 == taverns, 200 to 299 == shops
    public int currentPoiID;

    [SerializeField] private List<int> rewardPool;
    public List<Card> RewardPool { get; private set; }

    #endregion

    #region data memory

    private User tUser;
    private BattleArena tBattleArena;

    #endregion

    #region onAwake

    void Awake()
    {
        if (instance != null) Debug.LogError("Multiple Instances of GameManager!");
        else instance = this;

        List<Card> r = new();
        foreach (int i in rewardPool)
        {
            r.Add(new Card(i, 1, 1));
        }
        RewardPool = r;

        // User data is loaded locally when a login happens
        // rmv //
        usrData = new User("1234", "Pony");
        usrData.pid = 1;
        usrData.gold = 2000;
        usrData.friendsUID = new List<string> { "12234", "1999" };
        // ** //

        //LoadUserData();

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
                        usrData.characters[c].deck[d].lvl = usrData.cards[i].lvl;
                        break;
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
                    Debug.Log($"Pre-Add: Type: {usrData.cards[i].type}, Count: {usrData.cards[i].count}");
                    usrData.cards[i].count += c.count;
                    Debug.Log($"Post-Add: Type: {usrData.cards[i].type}, Count: {usrData.cards[i].count}");
                    if (usrData.cards[i].count > usrData.cards[i].RequiredCardsForUpgrade())
                    {
                        Debug.Log($"Pre-Upgrade: Type: {usrData.cards[i].type}, Count: {usrData.cards[i].count}");
                        usrData.cards[i].count -= usrData.cards[i].RequiredCardsForUpgrade();
                        usrData.cards[i].lvl += 1;
                        Debug.Log($"Used {usrData.cards[i].RequiredCardsForUpgrade()} to upgrade Card of type: {usrData.cards[i].type} to level: {usrData.cards[i].lvl} with remaining count: {usrData.cards[i].count}");
                    }
                    break;
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
        usrData = LoadUserData(usrData.uid);
    }

    // load any User by their ID
    public User LoadUserData(string id)
    {
        //return usrData;

        RestServerCaller.Instance.GetUserByIdRequestCall(id, SetLoadedUser);
        return tUser;
    }

    public void SaveUserData()
    {
        RestServerCaller.Instance.SendUpdateCall(usrData);
    }

    public void LoadPartyData()
    {
        // for testing purposes only:
        partyData = new Party(usrData);
        partyData.memberPoIids[0] = currentPoiID;

#warning missing

        // Correct method
        /*string url = "http://127.0.0.1:8000/api/partyById/" + usrData.pid + "/";
        RestServerCaller.Instance.GenericRequestCall(url, PartyCallback);*/
    }

    public void SaveBattleArena(BattleArena battleArena)
    {
        tBattleArena = battleArena;
#warning missing
        // get partyID and save battleArena to corresponding data base entry
    }

    public BattleArena LoadBattleArena() {
#warning missing
        // get partyID and load battleArena to corresponding data base entry
        return tBattleArena;
    }

    public void UpdatePlayerFlags(int index, bool value) {
#warning missing
        // get partyID and update the bool at the index in playerChecks[] in the corresponding BattleArena entry
    }

    public void UpdatePlayerCard(int index, Card card) {
#warning missing
        // get partyID and update the card at the index in playerCards[] in the corresponding BattleArena entry
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

        partyData = JsonUtility.FromJson<Party>(response.message);
        //Debug.Log(usrParty);
    }

    public void BattleArenaCallback(ServerMessage response) {
        if (response.IsError())
        {
            Debug.Log("Battle Arena doesn't exist yet...");
            tBattleArena = null;
            return;
        }

        tBattleArena = JsonUtility.FromJson<BattleArena>(response.message);
    }

    public void SetLoadedUser(User user)
    {
        if (user == null)
        {
            Debug.Log("User could not been found!");
            return;
        }
        tUser = user;
    }

    #endregion
}
