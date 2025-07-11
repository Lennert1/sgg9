using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class API : MonoBehaviour
{   void Start()
    {

    }

    // This function loads the user data from json as C# object so you can access the data 
    public User LoadUserDataFromFile()
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
    public void SaveUserDataToFile(User data)
    {
        // Saves the Userdata in a persistent data path (probably we don't have to worry about it...)
        string path = Application.persistentDataPath + "/userdata.json";

        // To JSON
        string json = JsonUtility.ToJson(data, true);

        System.IO.File.WriteAllText(path, json);

        Debug.Log("Userdaten gespeichert unter: " + path);
    }

}
