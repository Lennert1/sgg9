using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Mapbox.Json;
using Random = System.Random;
using System.Collections;

namespace GeoCoordinatePortable.gameLogic
{
    public class PartyManager
    {
        public static PartyManager Instance { get; private set; }
        
        private List<Party> allParties;

        public List<Party> fetchParties()
        {
            List<Party> parties = new List<Party>();
            allParties = parties;
#warning missing: rest call to fetch parties
            return parties;
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
            
        }

        public void leaveParty()
        {
#warning missing: rest call to leave party
            //call to leave party and let other members know you joined
            GameManager.Instance.partyData.members.Remove(GameManager.Instance.usrData.uid);
            GameManager.Instance.partyData = null;
        }

        //called if you're in a party and someone else used joinParty(yourPartyid)
        public void otherJoined(string uid)
        {
            GameManager.Instance.partyData.members.Add(uid);
        }

        //called if you're in a party and someone in your Party used leaveParty()
        public void otherLeft(string uid)
        {
            GameManager.Instance.partyData.members.Remove(uid);
        }
    }
}