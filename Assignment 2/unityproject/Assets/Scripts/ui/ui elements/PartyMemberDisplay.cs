using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PartyMemberDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI playerLevel;

    public void SetPlayerValues(User usr)
    {
        playerName.text = usr.name + "    (ID: " + usr.uid + ")";
        playerLevel.text = "LVL: " + usr.lvl;
    }
}
