using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProfileUI : UI
{
    [SerializeField] private TextMeshProUGUI labelName;
    [SerializeField] private TextMeshProUGUI labelID;
    [SerializeField] private TextMeshProUGUI labelLvl;

    public override void SetActive(bool b)
    {
        base.SetActive(b);

        if (b)
        {
            User usr = GameManager.Instance.usrData;
            labelName.text = usr.name;
            labelID.text = "id:" + usr.uid;
            labelLvl.text = "Level: " + usr.lvl;
        }
    }
}
