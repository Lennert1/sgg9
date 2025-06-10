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
                callback?.Invoke(JsonConvert.DeserializeObject<ServerMessage>(www.downloadHandler.text));
            }
        }

    }
}
