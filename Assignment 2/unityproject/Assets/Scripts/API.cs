using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Networking;

public class API : MonoBehaviour
{

    User loadedUser;
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


    public User GetLoadedUser()
    {
        return loadedUser;
    }

    // This function sends an update
    /*public IEnumerator SendUpdate(User updatedUser)
    {
        // Convert to json
        string jsonData = JsonUtility.ToJson(updatedUser);

        using (UnityWebRequest www = new UnityWebRequest("http://127.0.0.1:8000/api/updateData/", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

            // To let Unity know which data has been send in the body of the POST request
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);

            // For reading the server response
            www.downloadHandler = new DownloadHandlerBuffer();

            // Tell the server, that a json data was sent
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Response: " + www.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error: " + www.error);
            }
        }
    }*/

}
