using System.Collections;
using System.Collections.Generic;
using gamelogic.ServerClasses;
using Mapbox.Json;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager gameManager;


    #region fields

    [SerializeField] private UI activeUI;
    public User usrData;

    #endregion

    #region onAwake

    void Awake()
    {
        if (gameManager != null) Debug.LogError("Multiple Instances of GameManager!");
        else gameManager = this;
    }

    #endregion

    #region game logic

    public void SetUIActive(UI ui)
    {
        activeUI.SetActive(false);

        ui.SetActive(true);
        activeUI = ui;
    }

    #endregion

    #region data management

    public void LoadUserData()
    {
        RestServerCaller.Instance.GenericRequestCall("", UserCallback);
        Debug.LogWarning("URL in method LoadUserData() in class UI not yet added!");
    }

    public void UserCallback(ServerMessage response)
    {
        if (response.IsError())
        {
            Debug.Log("Error");
            return;
        }

        usrData = JsonConvert.DeserializeObject<User>(response.message);

        
    }

    #endregion

    #region static functions

    public static GameManager GetGameManager()
    {
        return gameManager;
    }
    
    #endregion
}
