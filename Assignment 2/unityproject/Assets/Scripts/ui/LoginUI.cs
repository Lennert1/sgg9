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
        Debug.Log("Pressed");
    }

    IEnumerator SendRegistration()
    {
        RegisterData data = new RegisterData
        {
            name = userInputField.text,
        };

        string jsonData = JsonUtility.ToJson(data);

        Debug.Log("JSON gesendet: " + jsonData); // ← Debug!

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

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Response: " + www.downloadHandler.text);
                errorMessage.text = "Registration successful! You can login now!";
            }
            else
            {
                Debug.LogError("Error: " + www.error);
            }
        }
    }

    IEnumerator SendLogin()
    {
        yield return null;
    }

}
