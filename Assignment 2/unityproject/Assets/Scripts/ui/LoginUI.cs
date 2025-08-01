﻿using Mapbox.Json;
using Mapbox.Map;
using System.Collections;

using TMPro;
using UnityEngine;
using UnityEngine.Networking;


public class LoginUI : UI
{
    [SerializeField] TMP_InputField userInputField;
    [SerializeField] TextMeshProUGUI errorMessage;

    [System.Serializable]
    public class RegisterData
    {
        public string name;
    }
    public void RegisterButtonPressed()
    {
        if (userInputField.text.Length <= 0)
        {
            errorMessage.text = "Empty input!";
            return;
        }
        StartCoroutine(SendRegistration());
    }

    public void LoginButtonPressed()
    {
        if (userInputField.text.Length <= 0)
        {
            errorMessage.text = "Empty input!";
            return;
        }
        StartCoroutine(SendLogin());
    }

    // This function sends the input of the text field to the server, where a new account is created
    IEnumerator SendRegistration()
    {
        User newUser = new User("-1", userInputField.text);

        string jsonData = JsonUtility.ToJson(newUser);

        using (UnityWebRequest www = new UnityWebRequest("http://127.0.0.1:8000/api/register/", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

            // To let Unity know which data has been send in the body of the POST request
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);

            // For reading the server response
            www.downloadHandler = new DownloadHandlerBuffer();

            // Tell the server, that a json data was sent
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            string response = www.downloadHandler.text;
            Debug.Log(response);

            if (www.result == UnityWebRequest.Result.Success)
            {
                errorMessage.text = "Registration successful! You can login now!";
            }
            else
            {
                if (www.responseCode == 409)
                {
                    errorMessage.text = "User already exists!";
                }
                else
                {
                    errorMessage.text = "An unknown error was found";
                    Debug.LogError("Error: " + www.error);

                }
                    
            }
        }


    }

    IEnumerator SendLogin()
    {
        RegisterData data = new RegisterData
        {
            name = userInputField.text,
        };

        string jsonData = JsonUtility.ToJson(data);


        using (UnityWebRequest www = new UnityWebRequest("http://127.0.0.1:8000/api/check_login/", "POST"))
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
                MapUI mapUI = GameObject.Find("UI").GetComponentInChildren<MapUI>(true);
                if (mapUI != null)
                {  
                    try
                    {
                        User userData = JsonUtility.FromJson<User>(www.downloadHandler.text);
                        //User userData = JsonConvert.DeserializeObject<User>(www.downloadHandler.text);
                        GameManager.Instance.usrData = userData;
                        LoadUI(mapUI);
                    } catch (JsonException e) 
                    {
                        Debug.Log("Das Json konnte nicht in einen User umgewandelt werden! Liegt das richtige Format vor?");
                    }
                    
                }
            }
            else
            {
                errorMessage.text = "User " + userInputField.text + " does not exist";
            }
        }
    }

    public void pressButton()
    {
        GameManager.Instance.LoadUserData("687a9657126b0b3d65c4d589");

    }


    

}
