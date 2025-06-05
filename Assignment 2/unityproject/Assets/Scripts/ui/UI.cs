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

    public void SetActive(bool v)
    {
        if (v) // activate
        {
            gameObject.SetActive(true);
        }
        else // deavtivate
        {
            gameObject.SetActive(false);
        }
    }

    public void LoadUI(UI ui)
    {
        gameManager.SetUIActive(ui);
    }
}
