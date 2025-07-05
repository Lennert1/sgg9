using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Mapbox.Json;
using Random = System.Random;

public class BattleManager : MonoBehaviour, ICardSelector
{
    public static BattleManager Instance { get; private set; }
    
    public Dungeon dungeon;
    private BattleArena battleArena;
    private BattleArenaUI battleArenaUI;
    public Enemy enemy;
    
    //uid, character, PoIID
    public List<Tuple<int, Character, int>> readyList = new List<Tuple<int,Character, int>>();
    private Character selectedCharacter;

    public int partyShield;
    
    public int partyHp;
    public int maxPartyHp;
    
    public int bossHp;
    public int maxBossHp;
    //uid, card
    private List<KeyValuePair<int, Card>> chosenCards = new List<KeyValuePair<int, Card>>();
    private List<Card> draw = new List<Card>();

    private bool playerTurn = true;

    public void StartBattle(BattleArena battleArena)
    {
        partyShield = GameManager.Instance.usrParty.shield;
        partyHp = GameManager.Instance.usrParty.hp;
        maxPartyHp = partyHp;
        enemy = battleArena.enemy;
        bossHp = enemy.hp;
        maxBossHp = enemy.hp;
        
        Debug.Log($"Battle Started! Party HP: {partyHp}, Boss HP: {bossHp}");
        Draw();
    }


    public void Draw()
    {
        if (selectedCharacter.deck.Count < 4)
        {
            battleArenaUI.SwitchToCardSelector(selectedCharacter.deck, partyHp, partyShield, bossHp);
        }
        else
        {
            this.draw = new List<Card>();

            foreach (Card card in selectedCharacter.deck)
            {
                draw.Add(card);
            }
            Random random = new Random();

            while (draw.Count > 4)
            {
                draw.RemoveAt( random.Next(0, draw.Count));
            }
            
            battleArenaUI.SwitchToCardSelector(selectedCharacter.deck, partyHp, partyShield, bossHp);
            
        }
    }

    private void PlayersPlayCards()
    {
        if (chosenCards.Count != GameManager.Instance.usrParty.memberCount)
        {
            Debug.Log("Some Players have not chosen cards!");
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

        ApplyCardEffectToParty(enemy.action());

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
        foreach (var card in chosenCards.Where(card => card.Key == GameManager.Instance.usrData.uid))
        {
            chosenCards.Remove(card);
            chosenCards.Add(new KeyValuePair<int, Card>(GameManager.Instance.usrData.uid, draw[p]));
            return;
        }

        chosenCards.Add(new KeyValuePair<int, Card>(GameManager.Instance.usrData.uid, draw[p]));

        /*
         Schicke Karte an andere
         */
        
        PlayersPlayCards();
    }
    //Wird gecallt wenn jemand anderes eine Karte ausgewählt hat
    public void OtherSelectCard(int uid, Card selectedCard)
    {
        foreach (var card in chosenCards.Where(card => card.Key == uid))
        {
            chosenCards.Remove(card);
            chosenCards.Add(new KeyValuePair<int, Card>(GameManager.Instance.usrData.uid, selectedCard));
            return;
        }
        
        chosenCards.Add(new KeyValuePair<int, Card>(uid, selectedCard));
        
        PlayersPlayCards();
    }
    
    #region readyFunctions
    public void Ready()
    {
        
        Debug.Log("Pressed Ready");
        //Falls man Leader ist, wird ready gecheckt
        if (GameManager.Instance.usrData.uid == GameManager.Instance.usrParty.members[0])
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
        if (GameManager.Instance.usrData.pid == GameManager.Instance.usrParty.members[0])
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
        if (GameManager.Instance.usrData.pid != GameManager.Instance.usrParty.members[0])
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
        if (readyList.Count == GameManager.Instance.usrParty.memberCount)
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
        List<Character> allCharacters = new List<Character>();
        foreach (var tuple in readyList)
        {
            allCharacters.Add(tuple.Item2);
        }
        this.battleArena = new BattleArena(allCharacters);
        this.dungeon = new Dungeon();
        this.dungeon.miniGameJson = JsonConvert.SerializeObject(battleArena);
        
        Debug.Log(this.dungeon.miniGameJson);
        /* schicke dungeon an die anderen
         
         schickeDungeon();
         
         */
        
        battleArenaUI = (BattleArenaUI) ((DungeonUI) GameManager.Instance.allUIs[6]).StartMiniGame(this.dungeon); //¯\_(ツ)_/¯
        StartBattle(battleArena);
    }
    //Bei nicht-Leadern
    public void allReady(Dungeon dungeon)
    {
        this.dungeon = dungeon;
        this.battleArena = JsonConvert.DeserializeObject<BattleArena>(dungeon.miniGameJson);
        
        battleArenaUI = (BattleArenaUI) ((DungeonUI) GameManager.Instance.allUIs[6]).StartMiniGame(dungeon); //¯\_(ツ)_/¯
        StartBattle(battleArena);
    }
    #endregion
    
    private void Awake()
    {
        Instance = this;
    }
}
