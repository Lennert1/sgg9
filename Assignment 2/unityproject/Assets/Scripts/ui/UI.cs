using gamelogic.ServerClasses;
using Mapbox.Unity.Location;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
