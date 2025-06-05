using System.Collections;
using System.Collections.Generic;
using gamelogic.ServerClasses;
using UnityEngine;
using Newtonsoft.Json;

public class UI : MonoBehaviour
{
    protected GameManager gameManager;

    void Awake()
    {
        gameManager = GameManager.GetGameManager();
    }

    public void SetActive(bool b)
    {
        gameObject.SetActive(b);
    }

    public void LoadUI(UI ui)
    {
        gameManager.SetUIActive(ui);
    }

    public void Unload()
    {
        SetActive(false);
    }
}
