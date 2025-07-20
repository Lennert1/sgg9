using System;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Json;
using Random = System.Random;
using System.Collections;

public class BattleManager : MonoBehaviour, ICardSelector
{
    public static BattleManager Instance { get; private set; }

    public BattleState battleState;

    public Dungeon dungeon;
    private BattleArena battleArena;
    private BattleArenaUI battleArenaUI;
    
    //uid, character, PoIID
    public List<Tuple<int, Character, int>> readyList = new List<Tuple<int,Character, int>>();
    private Character selectedCharacter;
    public List<Character> allCharacters;
    //uid, card
    private List<KeyValuePair<int, Card>> chosenCards = new List<KeyValuePair<int, Card>>();
    private List<Card> draw = new List<Card>();
    private List<Card> enemyCards = new();
    private List<Card> teamCards = new();

    private bool playerTurn = true;


    #region update data

    private bool running;
    [SerializeField] private float updateInterval = 1.0f;
    private int playerIndex;

    private IEnumerator UpdateData()
    {
        while (running)
        {
            BattleState oldState = battleArena.battleState;
            battleArena = GameManager.Instance.LoadBattleArena();

            switch (battleArena.battleState)
            {
                case BattleState.Waiting:
                    {
                        if (playerIndex == 0)
                        {
                            CheckReady();
                        }
                        break;
                    }
                case BattleState.Initial:
                    {
                        Debug.LogWarning("A BattleArena shouldn't be in BattleState.Initial");
                        break;
                    }
                case BattleState.SelectCard:
                    {
                        teamCards = new();
                        for (int i = 0; i < battleArena.playerCards.Count; i++)
                        {
                            if (battleArena.playerCards[i].type != -1)
                            {
                                Card c = new Card(battleArena.playerCards[i].type, battleArena.playerCards[i].lvl, 1);
                                teamCards.Add(c);
                                Debug.Log($"Card added to Team Cards of type: {c.type}");
                            }
                        }
                        Debug.LogWarning($"Played Cards: {teamCards.Count}");
                        battleArenaUI.DisplayTeamCards(teamCards);

                        if (oldState == BattleState.Waiting)
                        {
                            if (playerIndex != 0) allReady(battleArena);
                        }
                        if (oldState == BattleState.EvaluateRound)
                        {
                            Draw();
                        }
                        else
                        {
                            if (playerIndex == 0)
                            {
                                if (teamCards.Count >= GameManager.Instance.partyData.memberCount)
                                {
                                    EvaluateTeamCards();
                                }
                            }
                        }
                        break;
                    }
                case BattleState.EvaluateRound:
                    {
                        if (playerIndex != 0 && oldState != BattleState.EvaluateRound)
                        {
                            battleArenaUI.SwitchToEvaluation(battleArena.playerCards, battleArena.enemyCards, battleArena.teamHP, battleArena.TeamShield, battleArena.enemy.hp);
                        }
                        break;
                    }
                case BattleState.Lose:
                    {
                        if (playerIndex != 0 && oldState != BattleState.Lose)
                        {
                            battleArenaUI.SwitchToEvaluation(battleArena.playerCards, battleArena.enemyCards, battleArena.teamHP, battleArena.TeamShield, battleArena.enemy.hp);
                            battleArenaUI.SwitchToLossScreen();
                            running = false;
                        }
                        break;
                    }
                case BattleState.Win:
                    {
                        if (playerIndex != 0 && oldState != BattleState.Lose)
                        {
                            battleArenaUI.SwitchToEvaluation(battleArena.playerCards, battleArena.enemyCards, battleArena.teamHP, battleArena.TeamShield, battleArena.enemy.hp);
                            battleArenaUI.SwitchToWinScreen();
                            running = false;
                        }
                        break;
                    }
                case BattleState.Rewards:
                    {
                        Debug.LogWarning("A BattleArena shouldn't be in BattleState.Rewards");
                        break;
                    }
                default: Debug.LogError($"wtf is BattleState.{battleArena.battleState}"); break;
            }

            // === //

            yield return new WaitForSeconds(updateInterval);
        }
    }


    #region readyFunctions
    public void Ready()
    {
        Debug.Log("Pressed Ready");
        GameManager.Instance.LoadUserData();
        GameManager.Instance.LoadPartyData();

        for (int i = 0; i < GameManager.Instance.partyData.memberCount; i++) {
            if (GameManager.Instance.partyData.members[i] == GameManager.Instance.usrData.uid)
            {
                playerIndex = i;
                break;
            }
        }

        selectedCharacter = GameManager.Instance.usrData.characters[GameManager.Instance.usrData.selectedCharacter];
        GameManager.Instance.UpdateCardDeckLevels();

        if (GameManager.Instance.LoadBattleArena() == null || GameManager.Instance.LoadBattleArena().outOfUse)
        {
            battleArena = new(GameManager.Instance.partyData.memberCount);
            battleArena.playerChecks[playerIndex] = true;
            battleArena.battleState = BattleState.Waiting;
            GameManager.Instance.SaveBattleArena(battleArena);
        } else {
            GameManager.Instance.UpdatePlayerFlags(playerIndex, true);

            //Falls man Leader ist, wird ready gecheckt
            if (GameManager.Instance.usrData.uid == GameManager.Instance.partyData.members[0]) CheckReady();
        }

        running = true;
        StartCoroutine(UpdateData());
    }

    #endregion
    
    public void Unready()
    {
        GameManager.Instance.UpdatePlayerFlags(playerIndex, false);
    }

    public void CheckReady()
    {

        Debug.Log("Ready Check");

        bool ar = true;
        for (int i = 0; i < battleArena.playerChecks.Count; i++)
        {
            if (!battleArena.playerChecks[i] || GameManager.Instance.partyData.memberPoIids[i] != GameManager.Instance.partyData.memberPoIids[0])
            {
                ar = false;
                break;
            }
        }
        if (ar) allReady();
    }

    //Beim Leader
    private void allReady()
    {
        //
        allCharacters = new List<Character>();
        foreach (string id in GameManager.Instance.partyData.members)
        {
            User p = GameManager.Instance.LoadUserData(id);
            allCharacters.Add(p.characters[p.selectedCharacter]);
        }
        int teamHP = 0;
        foreach (Character c in allCharacters)
        {
            teamHP += c.hp;
        }

        int et = new Random().Next(GameAssetManager.Instance.Enemies.Count - 1);
        EnemyScriptableObject es = GameAssetManager.Instance.ReadEnemy(et);
        List<Card> ed = new();
        foreach (int i in es.deck)
        {
            ed.Add(new Card(i, 1, 1));
        }
        int el = 0;
        foreach (Character c in allCharacters) el += c.lvl;

        Enemy e = new Enemy(et, ed, es.baseHP, el);
        battleArena = new BattleArena(allCharacters, e);
        dungeon = new Dungeon(0);
        dungeon.miniGameJson = JsonConvert.SerializeObject(battleArena);

        battleArena.teamHP = teamHP;
        battleArena.TeamShield = 0;

        battleArena.battleState = BattleState.SelectCard;

        GameManager.Instance.SaveBattleArena(battleArena);

        battleArenaUI = (BattleArenaUI)((DungeonUI)GameManager.Instance.allUIs[6]).StartMiniGame(dungeon); //¯\_(ツ)_/¯
        battleArenaUI.InitiateBattleArena(battleArena);

        Draw();
    }
    //Bei nicht-Leadern
    public void allReady(BattleArena battleArena)
    {
        battleArenaUI = (BattleArenaUI)((DungeonUI)GameManager.Instance.allUIs[6]).StartMiniGame(dungeon); //¯\_(ツ)_/¯
        battleArenaUI.InitiateBattleArena(battleArena);
        Draw();
    }
    #endregion

    
    #region CardPlay
    public void Draw()
    {
        if (selectedCharacter.deck.Count < 4)
        {
            battleArenaUI.SwitchToCardSelector(selectedCharacter.deck, battleArena.teamHP, battleArena.TeamShield, battleArena.enemy.hp);
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

            battleArenaUI.SwitchToCardSelector(draw, battleArena.teamHP, battleArena.TeamShield, battleArena.enemy.hp);

        }
    }

    
    public void SelectCard(int p)
    {
        if (!playerTurn) return;

        Debug.Log($"Selected Card at index: {p}");

#warning remove following line as soon as the call after it is fully implemented!
        battleArena.playerCards[playerIndex] = draw[p];
        GameManager.Instance.UpdatePlayerCard(playerIndex, draw[p]);
    }
    
    private void EvaluateTeamCards()
    {
        playerTurn = false;
        Debug.Log("Playing Players cards");

        foreach (Card c in teamCards)
        {
            ApplyCardEffectToBoss(c);
        }
     
        if (battleArena.enemy.hp <= 0)
        {
            EndBattle(true);
            return;
        }

        EnemyPlayCards();

        foreach (Card c in battleArena.playerCards)
        {
            c.type = -1;
        }
        
        StartCoroutine(StartEvaluationPhase());
    }

    
    private void EnemyPlayCards()
    {
        Debug.Log("Playing Enemies cards");
        enemyCards = new(battleArena.enemy.deck);
        Random r = new();
        while (enemyCards.Count > 3)
        {
            enemyCards.RemoveAt(r.Next(enemyCards.Count));
        }

        ApplyCardEffectToParty(enemyCards);

        if (battleArena.teamHP <= 0)
        {
            EndBattle(false);
            return;
        }
    }

    public IEnumerator StartEvaluationPhase()
    {
        battleArenaUI.SwitchToEvaluation(teamCards, enemyCards, battleArena.teamHP, battleArena.TeamShield, battleArena.enemy.hp);

        battleArena.playerCards = teamCards;
        battleArena.enemyCards = enemyCards;
        battleArena.battleState = BattleState.EvaluateRound;
        GameManager.Instance.SaveBattleArena(battleArena);

        yield return new WaitForSeconds(10);
        playerTurn = true;
        Draw();
    }

    private void ApplyCardEffectToBoss(Card card)
    {
        CardScriptableObject scriptable = GameAssetManager.Instance.ReadCard(card.type);

        int damage = scriptable.GetDamage(card.lvl);
        int healing = scriptable.GetHealing(card.lvl);
        int shield = scriptable.GetShield(card.lvl);
        battleArena.enemy.hp -= damage;
        battleArena.teamHP += healing;
        battleArena.TeamShield += shield;

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
            if (battleArena.TeamShield > damage)
            {
                battleArena.TeamShield -= damage;
            }
            else
            {
                damage -= battleArena.TeamShield;
                battleArena.TeamShield = 0;
                battleArena.teamHP -= damage;
                shieldBreak = true;
            }

            battleArena.teamHP -= damage;
            battleArena.enemy.hp += healing;
            if (shieldBreak)
            {
                Debug.Log($"Boss broke the shield and dealt {damage} damage to Party\nBoss healed for {healing} HP");
            }
            else
            {
                if (battleArena.TeamShield > 0)
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
        battleArenaUI.SwitchToEvaluation(teamCards, enemyCards, battleArena.teamHP, battleArena.TeamShield, battleArena.enemy.hp);
        running = false;
        battleArena.outOfUse = true;
        if (victory)
        {
            Debug.Log("Victory!");
            battleArena.battleState = BattleState.Win;
            GameManager.Instance.SaveBattleArena(battleArena);
            battleArenaUI.SwitchToWinScreen();
        }
        else
        {
            Debug.Log("Defeat!");
            battleArena.battleState = BattleState.Lose;
            GameManager.Instance.SaveBattleArena(battleArena);
            battleArenaUI.SwitchToLossScreen();
        }
    }

    public void CollectRewards()
    {
        GameManager.Instance.usrData.gold += battleArena.rewardGold;
        GameManager.Instance.usrData.upgradePoints += battleArena.rewardUpgradePoints;

        GameManager.Instance.AddCardsToInventory(battleArena.rewardCards);

        GameManager.Instance.SaveUserData();
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
    readyList.Clear();
    selectedCharacter = null;
    allCharacters = null;
    
    chosenCards.Clear();
    draw.Clear();

    playerTurn = true;
        
    }
}
