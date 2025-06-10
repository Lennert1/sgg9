using System.Collections;
using System.Collections.Generic;
using gamelogic.ServerClasses;
using UnityEngine;
using Newtonsoft.Json;

public class UI : MonoBehaviour
{
    public virtual void SetActive(bool b)
    {
        gameObject.SetActive(b);
    }


    public virtual void Unload()
    {
        SetActive(false);
    }

    #region inputs

    public void LoadUI(UI ui)
    {
        GameManager.Instance.SetUIActive(ui);
    }

    #endregion
}
