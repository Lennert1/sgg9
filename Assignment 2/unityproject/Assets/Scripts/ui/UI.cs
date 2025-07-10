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

    // This function loads the user data from json as C# object so you can access the data 
    protected User LoadUserDataFromFile()
    {
        string path = Application.persistentDataPath + "/userdata.json";

        if (System.IO.File.Exists(path))
        {
            // Reads the json data and converts to a string
            string json = System.IO.File.ReadAllText(path);

            // Converts a jason to a User
            User data = JsonUtility.FromJson<User>(json);

            /*Debug.Log("Benutzername: " + data.name);
            Debug.Log("UID: " + data.uid);
            Debug.Log("Level: " + data.lvl); */

            return data;
        }
        else
        {
            Debug.LogWarning("Data not found" + path);
            return null;
        }
    }

    // Converts a C# User object to a json and overwrites the existing data
    protected void SaveUserDataToFile(User data)
    {
        // Saves the Userdata in a persistent data path (probably we don't have to worry about it...)
        string path = Application.persistentDataPath + "/userdata.json";

        // To JSON
        string json = JsonUtility.ToJson(data, true);

        System.IO.File.WriteAllText(path, json);

        Debug.Log("Userdaten gespeichert unter: " + path);
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
