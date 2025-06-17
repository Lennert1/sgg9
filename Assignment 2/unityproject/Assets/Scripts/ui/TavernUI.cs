using System.Collections;
using System.Collections.Generic;
using Mapbox.Unity.Map.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TavernUI : UI
{
    [SerializeField] Transform partyDisplayTranform;
    [SerializeField] private int positionOffset;
    [SerializeField] private GameObject partyPrefab;

    private List<Party> parties;
    private List<GameObject> partyDisplay = new List<GameObject>();

    public override void SetActive(bool b)
    {
        base.SetActive(b);

        if (b)
        {
            if (GameManager.Instance.usrData.pid != 0)
            {
                // display players party
            }
            else
            {
                LoadAvailableParties();
            }
        }
    }

    public override void Unload()
    {
        ClearPartyDisplay();

        base.Unload();
    }


    public void GoToMapClicked()
    {
        SceneManager.LoadScene(1);
    }

    private void LoadAvailableParties()
    {
#warning missing: REST-Call to load all parties from this tavern
        // for testing purposes only:
        parties = new List<Party>() { new Party(GameManager.Instance.usrData), new Party(new User(9865, "James")), new Party(new User(2354, "Kirk")), new Party(new User(9834, "Rob")) };

        List<Party> usrParties = new List<Party>();
        foreach (Party p in parties)
        {
            if (p.members[0] == GameManager.Instance.usrData.uid) usrParties.Add(p);
        }
        foreach (Party p in usrParties) parties.Remove(p);

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
    
    public void ClearPartyDisplay()
    {
        foreach (GameObject obj in partyDisplay)
        {
            Destroy(obj);
        }
    }
}
