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
            List<Party> parties = new List<Party> { new Party(new User("123", "Toast")), new Party(new User("124", "Toasty")), new Party(new User("125", "Toaster")) };
            allParties = parties;
#warning missing: rest call to fetch parties
        }

        public void createParty()
        {
            Party party = new Party(GameManager.Instance.usrData);
            GameManager.Instance.partyData = party;
            
#warning missing: rest call to register party
        }

        public void joinParty(int pid)
        {
#warning missing: rest call to join party
            //call to join party and let other members know you joined
            
            foreach (var p in allParties)
            {
                if (p.pid == pid)
                {
                    GameManager.Instance.partyData = p;
                    p.members.Add(GameManager.Instance.usrData.uid);
                }
            }
            
            loop = true;
            StartCoroutine(FetchPartiesLoop());

        }

        public void leaveParty()
        {
#warning missing: rest call to leave party
            //call to leave party and let other members know you joined
            GameManager.Instance.partyData.members.Remove(GameManager.Instance.usrData.uid);
            GameManager.Instance.partyData = null;
            loop = false;
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
        
        
        private void Awake()
        {
            Instance = this;
        }
        
    }
}