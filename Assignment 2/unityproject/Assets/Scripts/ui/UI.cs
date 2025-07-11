using gamelogic.ServerClasses;
using Mapbox.Unity.Location;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LoginUI;

public class UI : MonoBehaviour
{

    private UIEventManager _uiEventManager;
    protected API _api;

    private void Awake()
    {
        _api = GameObject.Find("UI").GetComponent<API>();
    }

    protected virtual void Start()
    {
        _uiEventManager = GameObject.Find("UI").GetComponent<UIEventManager>();
    }

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
        if(_uiEventManager != null)
        {
            _uiEventManager.StopAllTweensAndReset();
        }
        GameManager.Instance.SetUIActive(ui);
    }

    #endregion
}
