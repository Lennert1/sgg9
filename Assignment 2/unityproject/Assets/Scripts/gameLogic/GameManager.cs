using System.Collections;
using System.Collections.Generic;
using gamelogic.ServerClasses;
using Mapbox.Json;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
        private set
        {
            instance = value;
        }
    }
    private static GameManager instance;


    #region fields

    [SerializeField] private UI activeUI;
    [SerializeField] private UI[] allUIs;
    public User usrData;

    #endregion

    #region onAwake

    void Awake()
    {
        if (instance != null) Debug.LogError("Multiple Instances of GameManager!");
        else instance = this;

        LoadUserData();

        foreach (UI u in allUIs)
        {
            u.SetActive(false);
        }
        activeUI.SetActive(true);
    }

    #endregion

    #region game logic

    public void SetUIActive(UI ui)
    {
        activeUI.Unload();

        ui.SetActive(true);
        activeUI = ui;
    }

    #endregion

    #region data management

    public void LoadUserData()
    {
        // for testing purposes only:
        usrData = new User(1234, "Pony", new List<Card>() { new Card(16, 1, 1), new Card(1, 16, 1), new Card(1, 1, 16), new Card(1, 1, 1), new Card(16, 16, 16), new Card(16, 1, 16), new Card(1, 16, 16), new Card(16, 16, 1), new Card(16, 1, 1), new Card(1, 1, 16) }, new List<Character>());

        //RestServerCaller.Instance.GenericRequestCall("", UserCallback);
        Debug.LogWarning("URL in method LoadUserData() in class UI not yet added!");
    }

    public void SaveUserData()
    {
        //RestServerCaller.Instance.GenericSendCall("", usrData);
        Debug.LogWarning("URL in method SaveUserData() in class UI not yet added!");
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
}
