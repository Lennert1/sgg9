using System.Collections;
using System.Collections.Generic;
using gamelogic.ServerClasses;
using UnityEngine;
using Newtonsoft.Json;

public class UI : MonoBehaviour
{
    protected GameManager gameManager;

    User data;

    void Awake()
    {
        gameManager = GameManager.GetGameManager();
    }

    void LoadUserData()
    {
        RestServerCaller.Instance.GenericRequestCall("", UserCallback);
        Debug.LogWarning("Missing URL in method LoadUserData() in class UI");
    }

    public void UserCallback(ServerMessage response)
    {
        if (response.IsError())
        {
            Debug.Log("Error");
            return;
        }

        User usr = JsonConvert.DeserializeObject<User>(response.message);

        // process received data
    }
}
