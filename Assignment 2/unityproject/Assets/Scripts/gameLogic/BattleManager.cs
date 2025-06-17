using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }
    
    private Party party;
    private Dungeon dungeon;
    public Boss boss;
    
    public int partyHp;
    public int maxPartyHp;
    
    public int bossHp;
    public int maxBossHp;

    private Dictionary<int, Card> chosenCards = new Dictionary<int, Card>();

    private bool playerTurn = true;

    public void StartBattle(Party party, Dungeon dungeon)
    {
        this.party = party;
        partyHp = party.hp;
        maxPartyHp = partyHp;
        this.dungeon = dungeon;
        boss = dungeon.boss;
        bossHp = boss.hp;
        maxBossHp = boss.hp;
        
        Debug.Log($"Battle Started! Party HP: {partyHp}, Boss HP: {bossHp}");
    }


    public void PlayerChooseCard(int playerIndex, Card card)
    {
        if (!chosenCards.ContainsKey(playerIndex))
            chosenCards[playerIndex] = card;
        
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
    
    
}
