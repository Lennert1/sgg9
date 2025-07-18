using System.Collections;
using System.Collections.Generic;
using GeoCoordinatePortable.gameLogic;
using Mapbox.Unity.Map.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TavernUI : UI
{
    [SerializeField] private GameObject notInParty;
    [SerializeField] private GameObject inParty;

    [SerializeField] private int positionOffset;
    [SerializeField] Transform partyDisplayTranform;
    [SerializeField] private GameObject partyPrefab;

    [SerializeField] private Transform playerDisplayTransform;
    [SerializeField] private GameObject playerPrefab;

    private List<Party> parties;
    private List<GameObject> partyDisplay = new();

    private List<GameObject> playerDisplay = new();

    public override void SetActive(bool b)
    {
        base.SetActive(b);

        if (b)
        {
            GameManager.Instance.LoadUserData();

            if (GameManager.Instance.usrData.pid != 0)
            {
                notInParty.SetActive(false);
                inParty.SetActive(true);
                DisplayParty();
            }
            else
            {
                notInParty.SetActive(true);
                inParty.SetActive(false);
                DisplayAvailableParties();
            }
        }
    }

    public override void Unload()
    {
        ClearPartyDisplay();
        ClearPlayerDisplay();

        base.Unload();
    }

    public void PressCreateParty() {
        PartyManager.Instance.createParty();
        Debug.Log("Created Party!");

        notInParty.SetActive(false);
        inParty.SetActive(true);
        DisplayParty();
    }

    public void PressJoinParty(int pid) {
        PartyManager.Instance.joinParty(pid);
        Debug.Log("Joined Party!");

        notInParty.SetActive(false);
        inParty.SetActive(true);
        DisplayParty();
    }

    public void PressLeaveParty() {
        PartyManager.Instance.leaveParty();
        Debug.Log("Left Party!");

        notInParty.SetActive(true);
        inParty.SetActive(false);
        DisplayAvailableParties();
    }

    public void DisplayAvailableParties()
    {
        parties = PartyManager.Instance.fetchParties();

        List<Party> filter = new List<Party>();
        foreach (Party p in parties)
        {
            if (p.members[0] == GameManager.Instance.usrData.uid) filter.Add(p);
            else if (p.memberCount >= 4) filter.Add(p);
        }
        foreach (Party p in filter) parties.Remove(p);

        partyDisplay = new List<GameObject>();
        for (int i = 0; i < parties.Count; i++)
        {
            Vector3 pos = new Vector3(0, -i * positionOffset, 0);
            GameObject panel = Instantiate(partyPrefab, Vector3.zero, Quaternion.identity, partyDisplayTranform);
            panel.transform.localPosition = pos;

            PartyDisplayPanel displayPanel = panel.GetComponent<PartyDisplayPanel>();
            displayPanel.SetUI(this);
            displayPanel.SetPartyValues(parties[i]);

            partyDisplay.Add(panel);
        }
    }

    public void DisplayParty()
    {
        GameManager.Instance.LoadPartyData();
        Party party = GameManager.Instance.partyData;

        playerDisplay = new();
        for (int i = 0; i < party.memberCount; i++)
        {
            Vector3 pos = new Vector3(0, -i * positionOffset, 0);
            GameObject panel = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, playerDisplayTransform);
            panel.transform.localPosition = pos;

            PartyMemberDisplay displayPanel = panel.GetComponent<PartyMemberDisplay>();
            displayPanel.SetPlayerValues(GameManager.Instance.LoadUserData(party.members[i]));

            playerDisplay.Add(panel);
        }
    }

    public void ClearPartyDisplay()
    {
        foreach (GameObject obj in partyDisplay)
        {
            Destroy(obj);
        }
    }

    public void ClearPlayerDisplay() {
        foreach (GameObject obj in playerDisplay)
        {
            Destroy(obj);
        }
    }
}
