using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Mapbox.Json;
using Random = System.Random;
using System.Collections;

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
    public List<Character> allCharacters;

    public int partyShield;
    
    public int partyHp;
    public int maxPartyHp;
    
    public int bossHp;
    public int maxBossHp;
    //uid, card
    private List<KeyValuePair<int, Card>> chosenCards = new List<KeyValuePair<int, Card>>();
    private List<Card> draw = new List<Card>();
    private List<Card> enemyCards = new();

    private bool playerTurn = true;
    
    
    #region readyFunctions
    public void Ready()
    {
        
        Debug.Log("Pressed Ready");
        //Falls man Leader ist, wird ready gecheckt
        if (GameManager.Instance.usrData.uid == GameManager.Instance.usrParty.members[0])
        {
            readyList.Add(new Tuple<int, Character, int>(GameManager.Instance.usrData.uid, GameManager.Instance.usrData.characters[GameManager.Instance.usrData.selectedCharacter], GameManager.Instance.currentPoiID));
            selectedCharacter = GameManager.Instance.usrData.characters[GameManager.Instance.usrData.selectedCharacter];
            GameManager.Instance.UpdateCardDeckLevels();
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
        allCharacters = new List<Character>();
        foreach (var tuple in readyList)
        {
            allCharacters.Add(tuple.Item2);
        }

        int et = new Random().Next(GameAssetManager.Instance.Enemies.Count - 1);
        EnemyScriptableObject es = GameAssetManager.Instance.ReadEnemy(et);
        List<Card> ed = new();
        foreach (int i in es.deck)
        {
            ed.Add(new Card(i));
        }
        int el = 0;
        foreach (Character c in allCharacters) el += c.lvl;

        Enemy e = new Enemy(et, ed, es.baseHP, el);
        battleArena = new BattleArena(allCharacters, e);
        dungeon = new Dungeon();
        dungeon.miniGameJson = JsonConvert.SerializeObject(battleArena);
        
        Debug.Log(battleArena.enemy.hp);
        /* schicke dungeon an die anderen
         
         schickeDungeon();
         
         */
        
        battleArenaUI = (BattleArenaUI) ((DungeonUI) GameManager.Instance.allUIs[6]).StartMiniGame(dungeon); //¯\_(ツ)_/¯
        StartBattle(battleArena);
    }
    //Bei nicht-Leadern
    public void allReady(Dungeon dungeon)
    {
        this.dungeon = dungeon;
        battleArena = JsonConvert.DeserializeObject<BattleArena>(dungeon.miniGameJson);
        
        battleArenaUI = (BattleArenaUI) ((DungeonUI) GameManager.Instance.allUIs[6]).StartMiniGame(dungeon); //¯\_(ツ)_/¯
        StartBattle(battleArena);
    }
    #endregion
    
    
    public void StartBattle(BattleArena battleArena)
    {
        partyShield = GameManager.Instance.usrParty.shield;
        partyHp = 0;
        foreach (int i in GameManager.Instance.usrParty.members)
        {
            User t = GameManager.Instance.LoadUserData(i);
            partyHp += t.characters[t.selectedCharacter].hp;
        }
        maxPartyHp = partyHp;
        enemy = battleArena.enemy;
        bossHp = enemy.hp;
        maxBossHp = enemy.hp;
        
        Debug.Log($"Battle Started! Party HP: {partyHp}, Boss HP: {bossHp}");
        Draw();
    }

    
    #region CardPlay
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
                draw.RemoveAt(random.Next(0, draw.Count));
            }

            String s = "";
            foreach (Card card in draw)
            {
                s += "Type:" + card.type.ToString() + " Lvl:" + card.lvl.ToString() + "\n";
            }
            Debug.Log("Cards:\n" + s);

            battleArenaUI.SwitchToCardSelector(draw, partyHp, partyShield, bossHp);

        }
    }

    
    public void SelectCard(int p)
    {
        if (!playerTurn) return; 
        
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
        
        CheckCards();
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
        
        CheckCards();
    }
    
    private void CheckCards()
    {
        if (chosenCards.Count != GameManager.Instance.usrParty.memberCount)
        {
            Debug.Log("Some Players have not chosen cards!");
            return;
        }

        playerTurn = false;
        Debug.Log("Playing Players cards");
     
        foreach (var kvp in chosenCards)
        {
            Card card = kvp.Value;
            ApplyCardEffectToBoss(card);
        }
     
        if (bossHp <= 0)
        {
            EndBattle(true);
            return;
        }
             
        //Leader Berechnet Gegnerkarte(n) und schickt sie an Teammitglieder
             
        if (GameManager.Instance.usrData.uid == GameManager.Instance.usrParty.members[0])
        {
            EnemyPlayCards();
        }
    }

    
    private void EnemyPlayCards()
    {
        Debug.Log("Playing Enemies cards");
        enemyCards = new(enemy.deck);
        Random r = new();
        while (enemyCards.Count > 3)
        {
            enemyCards.RemoveAt(r.Next(enemyCards.Count));
        }
        /* Schickt Gegnerkarte an Teammitglieder

        schickeKarten(List<Card> cards);

         */

        ApplyCardEffectToParty(enemyCards);

        if (partyHp <= 0)
        {
            EndBattle(false);
            return;
        }

        Debug.Log("Next round");
        StartCoroutine(StartEvaluationPhase());
    }
    
    private void EnemyPlayCards(List<Card> cards)
    {
        Debug.Log("Playing Enemies cards");
        
        ApplyCardEffectToParty(cards);

        if (partyHp <= 0)
        {
            EndBattle(false);
            return;
        }

        Debug.Log("Next round");
        StartCoroutine(StartEvaluationPhase());
    }

    public IEnumerator StartEvaluationPhase()
    {
        List<Card> teamCards = new();
        foreach (var kvp in chosenCards) teamCards.Add(kvp.Value);
        battleArenaUI.SwitchToEvaluation(teamCards, enemyCards, partyHp, partyShield, bossHp);
        yield return new WaitForSeconds(10);
        playerTurn = true;
        chosenCards.Clear();
        Draw();
    }

    private void ApplyCardEffectToBoss(Card card)
    {
        CardScriptableObject scriptable = GameAssetManager.Instance.ReadCard(card.type);

        int damage = scriptable.GetDamage(card.lvl);
        int healing = scriptable.GetHealing(card.lvl);
        int shield = scriptable.GetShield(card.lvl);
        bossHp -= damage;
        partyHp += healing;
        partyShield += shield;

        Debug.Log($"Dealt {damage} damage to Boss\nHealed for {healing} HP\nAdded {shield} Shield");
    }

    private void ApplyCardEffectToParty(List<Card> cards)
    {
        foreach (Card card in cards)
        {
            CardScriptableObject scriptable = GameAssetManager.Instance.ReadCard(card.type);
            bool shieldBreak = false;

            int damage = scriptable.GetDamage(card.lvl);
            int healing = scriptable.GetHealing(card.lvl);
            if (partyShield > damage)
            {
                partyShield -= damage;
            }
            else
            {
                damage -= partyShield;
                partyShield = 0;
                partyHp -= damage;
                shieldBreak = true;
            }

            partyHp -= damage;
            bossHp += healing;
            if (shieldBreak)
            {
                Debug.Log($"Boss broke the shield and dealt {damage} damage to Party\nBoss healed for {healing} HP");
            }
            else
            {
                if (partyShield > 0)
                {
                    Debug.Log($"Boss dealt {damage} damage to Party's Shield\nBoss healed for {healing} HP");
                }
                else
                {
                    Debug.Log($"Boss dealt {damage} damage to Party\nBoss healed for {healing} HP");
                }
            }
        }
    }
    #endregion
    
    
    //Defeat screen oder rewards hier
    private void EndBattle(bool victory)
    {
        List<Card> teamCards = new();
        foreach (var kvp in chosenCards) teamCards.Add(kvp.Value);
        battleArenaUI.SwitchToEvaluation(teamCards, enemyCards, partyHp, partyShield, bossHp);
        playerTurn = false;
        if (victory)
        {
            Debug.Log("Victory!");
            battleArenaUI.SwitchToWinScreen();
            // extra aufruf um zu den rewards zu kommen:
            // benötigt in diesem script eine methode die von ui aufgerufen werden kann um mitzuteilen wenn rewards gebraucht werden
            // SwitchToRewards(...) muss dann aus diesem script aufgerufen werden
            // falls rewards in BattleArena.cs gepspeichert werden dann würd ich die methode ohne parameter machen und von dort lesen
        }
        else
        {
            Debug.Log("Defeat!");
            battleArenaUI.SwitchToLossScreen();
        }
    }

    public void CollectRewards()
    {
        GameManager.Instance.usrData.gold += battleArena.rewardGold;
        GameManager.Instance.usrData.upgradePoints += battleArena.rewardUpgradePoints;

        GameManager.Instance.AddCardsToInventory(battleArena.rewardCards);

#warning send user data to database
    }
    
    private void Awake()
    {
        Instance = this;
    }

    public void Clear()
    {
    
    dungeon = null;
    battleArena = null;
    battleArenaUI = null;
    enemy = null;
    readyList.Clear();
    selectedCharacter = null;
    allCharacters = null;

    partyShield = 0;
    
    partyHp = 0;
    maxPartyHp = 0;
    
    bossHp = 0;
    maxBossHp = 0;
    
    chosenCards.Clear();
    draw.Clear();

    playerTurn = true;
        
    }
}
