using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Mapbox.Json;
using Random = System.Random;

public class BattleManager : MonoBehaviour, ICardSelector
{
    public static BattleManager Instance { get; private set; }
    
    private Party party = new Party(GameManager.Instance.usrData);
    public Dungeon dungeon;
    private BattleArena battleArena;
    public Boss boss;
    
    //uid, character, PoIID
    public List<Tuple<int, Character, int>> readyList = new List<Tuple<int,Character, int>>();
    private Character selectedCharacter;

    public int partyShield;
    
    public int partyHp;
    public int maxPartyHp;
    
    public int bossHp;
    public int maxBossHp;

    private Dictionary<int, Card> chosenCards = new Dictionary<int, Card>();

    private bool playerTurn = true;

    public void StartBattle(BattleArena battleArena)
    {
        partyShield = party.shield;
        partyHp = party.hp;
        maxPartyHp = partyHp;
        boss = battleArena.boss;
        bossHp = boss.hp;
        maxBossHp = boss.hp;
        
        Debug.Log($"Battle Started! Party HP: {partyHp}, Boss HP: {bossHp}");
        PlayerChooseCard();
    }


    public void PlayerChooseCard()
    {
        if (selectedCharacter.deck.Count < 4)
        {
            //BattleArenaUI.SwitchToCardSelector(selectedCharacter.deck, partyHp, partyShield);
        }
        else
        {
            List<Card> draw = new List<Card>();

            foreach (Card card in selectedCharacter.deck)
            {
                draw.Add(card);
            }
            Random random = new Random();

            while (draw.Count > 4)
            {
                draw.RemoveAt( random.Next(0, draw.Count));
            }
            
            //BattleArenaUI.SwitchToCardSelector(selectedCharacter.deck, partyHp, partyShield);
            
        }
    }

    private void PlayersPlayCards()
    {
        if (chosenCards.Count != party.memberCount)
        {
            String msg = "Player IDs ";
            foreach (int i in party.members)
            {
                if (!chosenCards.ContainsKey(i))
                {   
                    msg += i + " ";
                }
            }
            
            Debug.Log($"{msg}have not chosen cards!");
            return;
        }
        
        Debug.Log("Playing Players cards");

        foreach (var kvp in chosenCards)
        {
            Card card = kvp.Value;
            ApplyCardEffectToBoss(card);
        }

        chosenCards.Clear();

        if (bossHp <= 0)
        {
            EndBattle(true);
            return;
        }

        EnemyPlayCards();
    }

    private void EnemyPlayCards()
    {
        Debug.Log("Playing Enemies cards");

        ApplyCardEffectToParty(boss.action());

        if (partyHp <= 0)
        {
            EndBattle(false);
            return;
        }

        Debug.Log("Next round");
    }

    private void ApplyCardEffectToBoss(Card card)
    {
        // bisher nur dmg
        int damage = card.type == 0 ? card.lvl * 10 : 0;
        bossHp -= damage;

        Debug.Log($"Player dealt {damage} damage. Boss HP: {bossHp}");
    }

    private void ApplyCardEffectToParty(Card card)
    {
        // bisher nur dmg
        int damage = card.type == 0 ? card.lvl * 15 : 0;
        partyHp -= damage;

        Debug.Log($"Boss dealt {damage} damage. Party HP: {partyHp}");
    }

    //Defeat screen oder rewards hier
    private void EndBattle(bool victory)
    {
        if (victory)
        {
            Debug.Log("Victory!");
        }
        else
        {
            Debug.Log("Defeat!");
        }
    }


    public void SelectCard(int p)
    {
        throw new NotImplementedException();
    }
    #region readyFunctions
    public void Ready()
    {
        
        Debug.Log("Pressed Ready");
        //Falls man Leader ist, wird ready gecheckt
        if (GameManager.Instance.usrData.uid == party.members[0])
        {
            readyList.Add(new Tuple<int, Character, int>(GameManager.Instance.usrData.uid, GameManager.Instance.usrData.characters[0], GameManager.Instance.currentPoiID));
            selectedCharacter = GameManager.Instance.usrData.characters[0];
            CheckReady();
        }
        else
        {
            /*schickt ready an den Leader und "ausgewählten" Character

            schickeAnLeader(int uid, Character c, int PoIID);

            */
        }
        
        
    }

    //Wird gecallt wenn sich wer anderes ready macht. Wird nur beim Leader aufgerufen
    public void OtherReady(int uid, Character c, int PoIID)
    {
        readyList.Add(new Tuple<int, Character, int>(uid, c, PoIID));
        CheckReady();
    }
    
    public void Unready()
    {        
        //Falls man selbst Leader ist
        if (GameManager.Instance.usrData.pid == party.members[0])
        {
            readyList.Remove(new Tuple<int, Character, int>(GameManager.Instance.usrData.pid,
                GameManager.Instance.usrData.characters[0], GameManager.Instance.currentPoiID));
        }
        else
        {
            /*schickt an den Leader das man nicht ready ist (soll auch mit exit button aufgerufen werden)
             
             schickeAnLeader(int uid);
             
             */
        }

        
    }
    
        //Wenn sich andere unready machen
        //Wird vom Leader gecallt falls man an einem anderen PoI ist
    public void OtherUnready(int uid)
    {
        if (GameManager.Instance.usrData.pid != party.members[0])
        {
            //Klickt exit Button
            return;
        }
        
        foreach (var tuple in readyList)
        {
            if (tuple.Item1 != uid)
            {
                readyList.Remove(tuple);
            }

            return;
        }
    }
    
    public void CheckReady()
    {

        Debug.Log("Ready Check");
        //wenn jeder ready ist und beim selben PoI generiert der Leader das dungeon
        if (readyList.Count == party.memberCount)
        {
            int PoIID = GameManager.Instance.currentPoiID;

            foreach (var tuple in readyList)
            {
                if (tuple.Item3 != PoIID)
                {
                    readyList.Remove(tuple);
                    /*
                     ruft OtherUnready() bei Partymitglied beim falschen PoI auf
                     */
                    return;
                }
            }
            
            allReady();
        }
    }
    //Beim Leader
    private void allReady()
    {
        //
        this.battleArena = new BattleArena();
        this.dungeon = new Dungeon();
        /* schicke dungeon an die anderen
         
         schickeDungeon();
         
         */
        
        ((DungeonUI) GameManager.Instance.allUIs[6]).StartMiniGame(this.dungeon); //¯\_(ツ)_/¯
        StartBattle(battleArena);
    }
    //Bei nicht-Leadern
    public void allReady(Dungeon dungeon)
    {
        this.dungeon = dungeon;
        this.battleArena = JsonConvert.DeserializeObject<BattleArena>(dungeon.miniGameJson);
        
        ((DungeonUI) GameManager.Instance.allUIs[6]).StartMiniGame(dungeon); //¯\_(ツ)_/¯
        StartBattle(battleArena);
    }
    #endregion
    public void changeParty(Party party)
    {
        this.party = party;
    }
    
    private void Awake()
    {
        Instance = this;
    }
}
