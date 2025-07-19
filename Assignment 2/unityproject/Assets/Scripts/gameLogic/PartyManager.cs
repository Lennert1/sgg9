using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using WebSocketSharp;


namespace GeoCoordinatePortable.gameLogic
{
    public class PartyManager : MonoBehaviour
    {
        private bool loop = false;
        public static PartyManager Instance { get; private set; }
        
        public List<Party> allParties;

        public void fetchParties()
        {
            StartCoroutine(RequestAllParties());
        }

        public void createParty()
        {
            Party party = new Party(GameManager.Instance.usrData);
            GameManager.Instance.partyData = party;
            StartCoroutine(RequestCreateParty());
            Debug.Log("Using Mock Party");
#warning missing: Server Implementation
        }

        public void joinParty(int pid)
        {
            StartCoroutine(RequestJoinParty(pid));
        }

        public void leaveParty()
        {
            StartCoroutine(RequestLeaveParty());
        }

        
        IEnumerator FetchPartiesLoop()
        {
            while (loop)
            {
                yield return StartCoroutine(FetchAllParties());

                // Warte 5 Sekunden, bevor die nächste Anfrage gesendet wird
                yield return new WaitForSeconds(5f);
            }
        }
        
        IEnumerator FetchAllParties()
        {
            Debug.Log("fetching all parties");
            TavernUI tavernUI = GameObject.Find("UI").GetComponentInChildren<TavernUI>(true);
            if (tavernUI != null)
            {
                tavernUI.DisplayAvailableParties();
            }
            yield return null;
        }

        public void Start()
        {
            loop = true;
            StartCoroutine(FetchPartiesLoop());
        }
        
        private void Awake()
        {
            Instance = this;
        }
        
        

        IEnumerator RequestAllParties()
        {
            
            using (UnityWebRequest www = new UnityWebRequest("http://127.0.0.1:8000/api/allParties/", "POST"))
            {
                /*
                
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes("");

                // To let Unity know which data has been send in the body of the POST request
                www.uploadHandler = new UploadHandlerRaw(bodyRaw);
                
                */
                
                // For reading the server response
                www.downloadHandler = new DownloadHandlerBuffer();

                // Tell the server, that a json data was sent
                www.SetRequestHeader("Content-Type", "application/json");

                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    
                    
                    try
                    {
                        List<Party> parties = JsonConvert.DeserializeObject<List<Party>>(www.downloadHandler.text);
                        allParties = parties;
                        
                        Debug.Log($"Parties Count: {parties.Count}");
                    } catch (JsonException e) 
                    {
                        Debug.Log("Das Json konnte nicht in einen User umgewandelt werden! Liegt das richtige Format vor?");
                    }
                }
                else
                {
                    Debug.Log($"{www.error}");
                }
            }
        }
        
        [System.Serializable]
        public class Change
        {
            public int pid;
            public String uid;
        }
        
        [System.Serializable]
        public class UID
        {
            public String uid;
        }
        IEnumerator RequestJoinParty(int pid)
        {
            
            using (UnityWebRequest www = new UnityWebRequest("http://127.0.0.1:8000/api/joinParty/", "POST"))
            {
                Change join = new Change{pid = pid, uid = GameManager.Instance.usrData.uid };

                String jsonData = JsonUtility.ToJson(join);
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
                    
                    
                    try
                    {
                        Party party = JsonConvert.DeserializeObject<Party>(www.downloadHandler.text);
                        GameManager.Instance.partyData = party;
                        GameManager.Instance.usrData.pid = party.pid;
                        Debug.Log($"Joined Party with pid: {party.pid}");
                    } catch (JsonException e) 
                    {
                        Debug.Log("Das Json konnte nicht in einen User umgewandelt werden! Liegt das richtige Format vor?");
                    }
                }
                else
                {
                    Debug.Log($"{www.error}");
                }
            }
        }
        IEnumerator RequestLeaveParty()
        {
            
            using (UnityWebRequest www = new UnityWebRequest("http://127.0.0.1:8000/api/leaveParty/", "POST"))
            {
                Change join = new Change{pid = GameManager.Instance.partyData.pid, uid = GameManager.Instance.usrData.uid };

                String jsonData = JsonUtility.ToJson(join);
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
                    
                    
                    try
                    {
                        GameManager.Instance.partyData = null;
                        GameManager.Instance.usrData.pid = 0;
                        Debug.Log($"Left Party");
                    } catch (JsonException e) 
                    {
                        Debug.Log("Das Json konnte nicht in einen User umgewandelt werden! Liegt das richtige Format vor?");
                    }
                }
                else
                {
                    Debug.Log($"{www.error}");
                }
            }
        }
        IEnumerator RequestCreateParty()
        {
            
            using (UnityWebRequest www = new UnityWebRequest("http://127.0.0.1:8000/api/createParty/", "POST"))
            {
                UID create = new UID{uid = GameManager.Instance.usrData.uid };

                String jsonData = JsonUtility.ToJson(create);
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
                    
                    
                    try
                    {
                        Party party = JsonConvert.DeserializeObject<Party>(www.downloadHandler.text);
                        GameManager.Instance.partyData = party;
                        GameManager.Instance.usrData.pid = party.pid;
                        Debug.Log($"Created Party with pid: {party.pid}");
                    } catch (JsonException e) 
                    {
                        Debug.Log("Das Json konnte nicht in einen User umgewandelt werden! Liegt das richtige Format vor?");
                    }
                }
                else
                {
                    Debug.Log($"{www.error}");
                }
            }
        }
        
    }
}