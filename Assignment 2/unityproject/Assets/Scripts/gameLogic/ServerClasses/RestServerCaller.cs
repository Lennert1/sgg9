using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using WebSocketSharp;


namespace gamelogic.ServerClasses
{


    public class RestServerCaller : SingletonPersistant<RestServerCaller>
    {

        //delegate für eigens definierte Funktionen: parameter ServerMessage, return void
        //werden nach dem fertigen ausführen eines REST-Calls aufgerufen, dürfen weggelassen werden
        public delegate void ServerRequestCallBack(ServerMessage response);


        //Coroutine-Starter für Rest-Calls
        public void GenericSendCall(string url, Dictionary<string, object> values,
            ServerRequestCallBack callback = null)
        {
            StartCoroutine(GenericSend(url, values, callback));
        }

        public void GenericSendCall(string url, object values,
            ServerRequestCallBack callback = null)
        {
            StartCoroutine(GenericSend(url, values, callback));
        }

        public void GenericSendCall(string url, WWWForm form, ServerRequestCallBack callback = null)
        {
            StartCoroutine(GenericSend(url, form, callback));
        }

        public void GenericRequestCall(string url, ServerRequestCallBack callback = null)
        {
            StartCoroutine(GenericRequest(url, callback));
        }

        /*public void GetUserByIdRequestCall(string url,int id, ServerRequestCallBack onSuccess = null)
        {
            StartCoroutine(GetUserByID(id, onSuccess));
        }*/
        public void GetUserByIdRequestCall(int id, Action<User> onSuccess)
        {
            StartCoroutine(GetUserByID(id, onSuccess));
        }





        //Tatsächliche REST_Calls, als Coroutine Aufrufen

        private IEnumerator GenericSend(string url, Dictionary<string, object> values,
            ServerRequestCallBack callback = null)
        {
            //little safety check
            if (!url.StartsWith(TemplateSettings.url))
                url = TemplateSettings.url + url;
            using (var www = UnityWebRequest.Post(url, JsonConvert.SerializeObject(values), "application/json"))
            {
                yield return www.SendWebRequest();
                if (www.downloadHandler.text.IsNullOrEmpty())
                    throw new Exception("Server did not respond. Is the server up? or does it receive the request?");
                ///Debug.Log(www.downloadHandler.text);
                Debug.Log(JsonConvert.DeserializeObject<ServerMessage>(www.downloadHandler.text));
                callback?.Invoke(JsonConvert.DeserializeObject<ServerMessage>(www.downloadHandler.text));
            }
        }

        private IEnumerator GenericSend(string url, System.Object values,
            ServerRequestCallBack callback = null)
        {
            //little safety check
            if (!url.StartsWith(TemplateSettings.url))
                url = TemplateSettings.url + url;
            using (var www = UnityWebRequest.Post(url, JsonConvert.SerializeObject(values), "application/json"))
            {
                yield return www.SendWebRequest();
                if (www.downloadHandler.text.IsNullOrEmpty())
                    throw new Exception("Server did not respond. Is the server up? or does it receive the request?");
                Debug.Log(www.downloadHandler.text);
                Debug.Log(JsonConvert.DeserializeObject<ServerMessage>(www.downloadHandler.text));
                callback?.Invoke(JsonConvert.DeserializeObject<ServerMessage>(www.downloadHandler.text));
            }
        }

        private IEnumerator GenericSend(string url, WWWForm form, ServerRequestCallBack callback = null)
        {
            //little safety check
            if (!url.StartsWith(TemplateSettings.url))
                url = TemplateSettings.url + url;

            using (var www = UnityWebRequest.Post(url, form))
            {
                yield return www.SendWebRequest();
                if (www.downloadHandler.text.IsNullOrEmpty())
                    throw new Exception("Server did not respond. Is the server up? or does it receive the request?");
                Debug.Log(www.downloadHandler.text);
                callback?.Invoke(JsonConvert.DeserializeObject<ServerMessage>(www.downloadHandler.text));
            }
        }

        private IEnumerator GenericRequest(string url, ServerRequestCallBack callback = null)
        {
            //little safety check
            if (!url.StartsWith(TemplateSettings.url))
                url = TemplateSettings.url + url;
            using (var www = UnityWebRequest.PostWwwForm(url, ""))
            {
                yield return www.SendWebRequest();
                if (www.downloadHandler.text.IsNullOrEmpty())
                    throw new Exception("Server did not respond. Is the server up? or does it receive the request?");
                Debug.Log(www.downloadHandler.text);

                // This shit made me lose my mind
                //callback?.Invoke(JsonConvert.DeserializeObject<ServerMessage>(www.downloadHandler.text));
                ServerMessage serverMessage = new ServerMessage(www.downloadHandler.text, null, "MESSAGE");
                callback?.Invoke(serverMessage);
            }
        }

        // Corresponding method for Game manager LoadUserData(id) function
        // Every loaded user is first stored in loadedUser variable cuz Idk how to make that better
        private IEnumerator GetUserByID(int id, Action<User> onSuccess)
        {
            string url = "http://127.0.0.1:8000/api/userById/" + id + "/";
            UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                Debug.Log("Response: " + json);

                // To json
                User user = JsonUtility.FromJson<User>(json);
                onSuccess?.Invoke(user);
            }
            else
            {
                Debug.LogError("Error: " + request.error);
                onSuccess?.Invoke(null);
            }
        } 

    }
}
