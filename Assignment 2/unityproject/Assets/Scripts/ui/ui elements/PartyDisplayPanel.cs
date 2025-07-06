using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PartyDisplayPanel : MonoBehaviour
{
    private TavernUI ui;


    [SerializeField] TextMeshProUGUI labelTopText;
    [SerializeField] TextMeshProUGUI labelBottomText;

    private Party party;

    public void JoinParty()
    {
        int uID = GameManager.Instance.usrData.uid;
#warning missing: game logic call to join party with uID and partyID

        ui.PressJoinParty();
    }

    public void SetUI(TavernUI ui) { this.ui = ui; }

    public void SetPartyValues(Party p) {
        party = p;

        labelTopText.text = "Leader: " + p.members[0];
        labelBottomText.text = p.memberCount + " / 4 Members   ♥ " + p.hp + "   Shield: " + p.shield;
    }
}
